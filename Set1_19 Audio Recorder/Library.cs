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
    private const string file_name = "audoi.m4a";

    private string _filename;
    // MediaCapture은 애플리케이션에 의해 소리를 녹음(캡처)하는데 사용된다.
    private MediaCapture _capture;
    private InMemoryRandomAccessStream _buffer;

    public bool Recording;

    // 녹음을 하는데 있어 필요한 구성과 이벤트를 세팅하는 메소드이다
    // 또한 RecordLimitationExceeded, Failed가 일어났을떄의 예외처리도 포함한다.
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
                StreamingCaptureMode = StreamingCaptureMode.Audio
            };
            _capture = new MediaCapture();
            await _capture.InitializeAsync(settings);
            _capture.RecordLimitationExceeded += (MediaCapture sender) =>
             {
                 Stop();
                 throw new Exception(string.Format("Exceed Record Limitation"));
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

    // MediaCapture의 StartRecordToStreamAsync를 사용하여 오디오 녹음을 시작하는 메소드이다.
    public async void Record()
    {
        await Init();
        await _capture.StartRecordToStreamAsync(MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto), _buffer);
        if (Recording) throw new InvalidOperationException("Cannot execute two recodings at the same time!");
        Recording = true;
    }

    public async void Stop()
    {
        await _capture.StopRecordAsync();
        Recording = false;
    }

    // MediaElement Control을 이용해서 녹음했던 파일을 재생하는 메소드이다.
    public async Task Play(CoreDispatcher dispatcher)
    {
        MediaElement playback = new MediaElement();
        IRandomAccessStream audio = _buffer.CloneStream();
        if (audio == null) throw new ArgumentNullException("buffer");
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
                await RandomAccessStream.CopyAndCloseAsync(audio.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                await audio.FlushAsync();
                audio.Dispose();
            }
            IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read);
            playback.SetSource(stream, storageFile.FileType);
            playback.Play();
        });
    }
}