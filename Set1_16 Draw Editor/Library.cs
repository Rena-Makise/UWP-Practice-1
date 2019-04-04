using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

public class Library
{
    private const string app_title = "Draw Editor";
    private const string file_extension = ".drw";

    // Color를 string값으로 변환
    private string ToString(Color value)
    {
        return $"{value.A:X2}{value.R:X2}{value.G:X2}{value.B:X2}";
    }

    // string값을 Color로 변환
    private Color FromSting(string value)
    {
        return Color.FromArgb(
        Byte.Parse(value.Substring(0, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(2, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber),
        Byte.Parse(value.Substring(6, 2), NumberStyles.HexNumber));
    }

    // 두 개의 버튼이 있는 dialog를 생성
    public async Task<bool> ConfirmAsync(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    public void Init(ref InkCanvas display, ref ComboBox size, ref ComboBox colour)
    {
        string selectedSize = ((ComboBoxItem)size.SelectedItem).Tag.ToString();
        string selectedColour = ((ComboBoxItem)colour.SelectedItem).Tag.ToString();
        InkDrawingAttributes attributes = new InkDrawingAttributes
        {
            Color = FromSting(selectedColour),
            Size = new Windows.Foundation.Size(int.Parse(selectedSize), int.Parse(selectedSize)),
            IgnorePressure = false,
            FitToCurve = true
        };
        display.InkPresenter.UpdateDefaultDrawingAttributes(attributes);
        display.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;
    }

    // InkDrawingAttributes의 컬러 속성 설정
    public void Colour(ref InkCanvas display, ref ComboBox colour)
    {
        if (display != null)
        {
            string selectedColour = ((ComboBoxItem)colour.SelectedItem).Tag.ToString();
            InkDrawingAttributes attributes = display.InkPresenter.CopyDefaultDrawingAttributes();
            attributes.Color = FromSting(selectedColour);
            display.InkPresenter.UpdateDefaultDrawingAttributes(attributes);
        }
    }

    // InkDrawingAttributes의 사이즈 속성 설정
    public void Size(ref InkCanvas display, ref ComboBox size)
    {
        if(display!=null)
        {
            string selectedSize = ((ComboBoxItem)size.SelectedItem).Tag.ToString();
            InkDrawingAttributes attributes = display.InkPresenter.CopyDefaultDrawingAttributes();
            attributes.Size = new Size(int.Parse(selectedSize), int.Parse(selectedSize));
            display.InkPresenter.UpdateDefaultDrawingAttributes(attributes);
        }
    }

    // 새로운 드로잉을 생성할건지 물어보고 InkPresenter에서 StrokeContainer를 클리어
    public async void New(InkCanvas display)
    {
        if (await ConfirmAsync("Create New Drawing?", app_title, "Yes", "No"))
        {
            display.InkPresenter.StrokeContainer.Clear();
        }
    }

    // 파일에 포함되어 있는 InkPresenter의 StrokeContainer를 InkCanvas에 설정하는데 사용된다.
    // OpenSequentialReadAsync와 LoadAsync를 사용하여 FileOpenPicker와 함께 연다.
    public async void OpenAsync(InkCanvas display)
    {
        try
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(file_extension);
            StorageFile file = await picker.PickSingleFileAsync();
            using (IInputStream stream = await file.OpenSequentialReadAsync())
            {
                await display.InkPresenter.StrokeContainer.LoadAsync(stream);
            }
        }
        catch
        {
        }
    }

    // InkCanvas에 그려진 InkPresenter의 StrokeContainer 컨텐츠를 파일에 저장하는데 사용
    // FileSavePicker와 SaveAsync를 사용하여 저장
    public async void SaveAsync(InkCanvas display)
    {
        try
        {
            FileSavePicker picker = new FileSavePicker
            {
                DefaultFileExtension = file_extension,
                SuggestedFileName = "Drawing",
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeChoices.Add("Drawing", new List<string>() { file_extension });
            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await display.InkPresenter.StrokeContainer.SaveAsync(stream);
                }
            }
        }
        catch
        {
        }
    }
}