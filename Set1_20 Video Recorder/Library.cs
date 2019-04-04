using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

public class Library
{
    private const string file_name = "video.mp4";

    private string _filename;
    // 녹음과 마찬가지로 MediaCapture는 연결된 카메라나 웹캠으로부터 비디오를 캡쳐하는데 사용된다.
    private MediaCapture _capture;
    private InMemoryRandomAccessStream _buffer;

    public bool Recording;

    // 녹화을 하는데 있어 필요한 구성과 이벤트를 세팅하는 메소드이다
    // 또한 RecordLimitationExceeded, Failed가 일어났을떄의 예외처리도 포함한다. (녹음과 거의 유사하다)
    // 웬진 모르겠지만 모바일에서는 카메라를 잡질 못하는것 같다. 영상 녹화가 되지 않는다.
    private async Task<bool> Init()
    {
        if (_buffer != null)
        {
            _buffer.Dispose();
        }
        _buffer = new InMemoryRandomAccessStream();
        if (_capture != null)
        {
            _capture.Dispose();
        }
        try
        {
            MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings
            {
                StreamingCaptureMode = StreamingCaptureMode.AudioAndVideo
            };
            _capture = new MediaCapture();
            await _capture.InitializeAsync();
            _capture.RecordLimitationExceeded += (MediaCapture sender) =>
            {
                Stop();
                throw new Exception("Exceeded Record Limitation");
            };
            _capture.Failed += (MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs) =>
            {
                Recording = false;
                throw new Exception(string.Format("Code: {0}. {1}", errorEventArgs.Code, errorEventArgs.Message));
            };
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null && ex.InnerException.GetType() == typeof(UnauthorizedAccessException))
            {
                throw ex.InnerException;
            }
            throw;
        }
        return true;
    }

    // MediaCapture의 StartRecordToStreamAsync를 사용하여 비디오 녹화를 시작하는 메소드이다.
    public async void Record(CaptureElement preview)
    {
        await Init();
        preview.Source = _capture;
        await _capture.StartPreviewAsync();
        await _capture.StartRecordToStreamAsync(MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto), _buffer);
        if (Recording) throw new InvalidOperationException("Cannot execute two recordings at the same time");
        Recording = true;
    }

    public async void Stop()
    {
        await _capture.StopRecordAsync();
        Recording = false;
    }

    // MediaElement Control을 이용해서 녹화했던 비디오를 재생하는 메소드이다.
    public async Task Play(CoreDispatcher dispatcher, MediaElement playback)
    {
        IRandomAccessStream video = _buffer.CloneStream();
        if (video == null) throw new ArgumentNullException("buffer");
        StorageFolder storageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        if (!string.IsNullOrEmpty(_filename))
        {
            StorageFile original = await storageFolder.GetFileAsync(_filename);
            await original.DeleteAsync();
        }
        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
        {
            StorageFile storageFile = await storageFolder.CreateFileAsync(file_name, CreationCollisionOption.GenerateUniqueName);
            _filename = storageFile.Name;
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await RandomAccessStream.CopyAndCloseAsync(video.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                await video.FlushAsync();
                video.Dispose();
            }
            IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read);
            playback.SetSource(stream, storageFile.FileType);
            playback.Play();
        });
    }
}