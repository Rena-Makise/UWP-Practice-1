using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

public class Libraray
{
    private const string app_title = "Text Editor";
    private const string file_extension = ".txt";

    // 두개의 버튼이 있는 dialog을 만드는데 사용된다.
    // NewAsync에서 사용
    private async Task<bool> ConfirmAsync(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    // 새 문서를 만들지 확인
    // Yes일시에 TextBox의 내용이 빈 문자열로 설정된다.
    public async void NewAsync(TextBox display)
    {
        if (await ConfirmAsync("Create New Document?", app_title, "Yes", "No"))
        {
            display.Text = string.Empty;
        }
    }

    // TextBox의 내용을 파일의 내용으로 설정하는데 사용
    // 파일은 FileOpenPicker로 열려서 ReadTextAsync로 읽힌다.
    public async void OpenAsync(TextBox display)
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(file_extension);
            StorageFile file = await picker.PickSingleFileAsync();
            display.Text = await FileIO.ReadTextAsync(file);
        }
        catch
        {
        }
    }

    // TextBox에 있는 컨텐츠가 파일에 저장되는데 사용
    // FileSavePicker와 WriteTextAsync를 통해 파일에 저장
    public async void SaveAsync(TextBox display)
    {
        try
        {
            FileSavePicker picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                DefaultFileExtension = file_extension,
                SuggestedFileName = "Document",
            };
            picker.FileTypeChoices.Add("Plain Text", new List<string>() { file_extension });
            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, display.Text);
            }
        }
        catch
        {
        }
    }
}