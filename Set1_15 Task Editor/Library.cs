using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

[DataContract]
public class Task
{
    [DataMember]
    public bool Done { get; set; }

    [DataMember]
    public string Text { get; set; }
}

public class Library
{
    private const string app_title = "Task Editor";
    private const string file_extension = ".tsk";

    private ObservableCollection<Task> _list = new ObservableCollection<Task>();
    DataContractJsonSerializer _serialiser = new DataContractJsonSerializer(typeof(ObservableCollection<Task>));

    // 두 개의 버튼이 있는 대화상자를 만드는데 사용
    public async Task<bool> ConfirmAsync(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    public void Add(ref ListBox display, string value, KeyRoutedEventArgs args)
    {
        if (args.Key == Windows.System.VirtualKey.Enter)
        {
            if (display.SelectedIndex > -1)
            {
                // ObservableCollection에서 ListBox의 SelectedIndex위치에 항목을 넣는다.
                _list.Insert(display.SelectedIndex, new Task() { Text = value }); // Insert at Selected
            }
            else
            {
                // 또는 ObservableCollection의 끝에 항목을 넣는다.
                _list.Add(new Task() { Text = value }); // Add at End
            }
            display.ItemsSource = _list;
            display.Focus(FocusState.Keyboard);
        }
    }

    // ObservableCollection에서 ListBox의 SelectedIndex위치에 있는 항목을 제거한다. 
    public void Remove(ref ListBox display)
    {
        if (display.SelectedIndex > -1)
        {
            _list.RemoveAt(display.SelectedIndex);
            display.ItemsSource = _list;
        }
    }

    // 새 태스크 리스트를 만들지 여부를 확인
    // "Yes"시 ListBox의 컨텐츠가 비워진다.
    public async void New(ListBox display)
    {
        if (await ConfirmAsync("Create New List?", app_title, "Yes", "No"))
        {
            _list.Clear();
            display.ItemsSource = _list;
            display.Focus(FocusState.Keyboard);
        }
    }

    // 파일의 내용을 ObservableCollection의 컨텐츠로 불러오는데 사용
    // DataContractJsonSerializer와 OpenStreamForReadAsync을 사용하여 FileOpenPicker를 통해 파일을 열고,
    // ListBox에 데이터가 바인딩된다.
    public async void OpenAsync(ListBox display)
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(file_extension);
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                _list = (ObservableCollection<Task>)_serialiser.ReadObject(await file.OpenStreamForReadAsync());
                display.ItemsSource = _list;
            }
        }
        catch
        {

        }
    }

    // ObservableCollection의 컨텐츠를 가져와서 FileSavePicker와 OpenStreamForWriteAsync를 통해 파일에 저장한다
    public async void SaveAsync(ListBox display)
    {
        try
        {
            FileSavePicker picker = new FileSavePicker
            {
                DefaultFileExtension = file_extension,
                SuggestedFileName = "Tasks",
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };
            picker.FileTypeChoices.Add("Task List", new List<string>() { file_extension });

            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                _serialiser.WriteObject(await file.OpenStreamForWriteAsync(), _list);
            }
        }
        catch
        {
        }
    }
}