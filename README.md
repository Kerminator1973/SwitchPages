# Переключение страниц в приложении Avalonia (Navigator)

Задачей данной примера является организация пользовательского интерфейса с использованием переключения между отдельными страницами приложения с использованием навигационного механизма. По сути, здесь осуществляется попытка реализовать поведение приложения соответствующего типовым решениям в React, или Blazor-приложений.

Основная идея состоит в том, что в основном View (он же - "Host" View), который по традиции называется "MainWindow.axaml" находится ContentControl. Этот ContentControl является placeholder-ом, в котором размещается текузая отбражаемая пара ViewModel и View. Вот так организуется верстка:

```csharp
<DockPanel>
    <!-- Навигационная панель -->
    <StackPanel DockPanel.Dock="Left" Background="LightGray" Spacing="10">
        <Button Content="Home" Command="{Binding GoToHomeCommand}" />
        <Button Content="Settings" Command="{Binding GoToSettingsCommand}" />
    </StackPanel>

    <!-- Текущая отображаемая пара ViewModel + View -->
    <ContentControl Content="{Binding CurrentPageViewModel}" />

</DockPanel>	
```

Связанные с MainWindow компонент ViewModel (MainWindowViewModel.cs) выполняет задачи оркестрации контентом, т.е. именно этот класс отвечает за переключение между разными парами ViewModel и Views:

```csharp
public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPageViewModel;

    private readonly HomePageViewModel _homePageViewModel;
    private readonly SettingsPageViewModel _settingsPageViewModel;

    public MainWindowViewModel()
    {
        _homePageViewModel = new HomePageViewModel();
        _settingsPageViewModel = new SettingsPageViewModel();

        _currentPageViewModel = _homePageViewModel;
    }

    [RelayCommand]
    private void GoToHome()
    {
        CurrentPageViewModel = _homePageViewModel;
    }

    [RelayCommand]
    private void GoToSettings()
    {
        CurrentPageViewModel = _settingsPageViewModel;
    }
}
```

Логика MainWindowViewModel такова: класс реализует обработку команд навигационной панели и переключает CurrentPageViewModel на тот View, который нам необходим для отображения в данный момент.

Следует заметить, что класс MainWindowViewModel ничего не знает о Views, он работает только с ViewModels и именно в этом основная идея данного шаблона.

Мы реализуем нужные нам пары ViewModels и Views в соответствующих папках "ViewModels" и "Views". Соответствующим образом имеет смысл определить и пространства имён.

В данном примере, для простоты, используется всего две страницы: "HomePage" и "SettingsPage". В реализации этих страниц нет ничего особенного - это обычные Views и ViewModels.

Ключевая идея шаблона реализуется в файле "App.axaml". В типовом случае, в этом файле, при создании проекта, генерируется вот такой код:

```csharp
<Application.DataTemplates>
    <local:ViewLocator/>
</Application.DataTemplates>
```

Магия возникает, если мы заменяем его на следующий код:

```csharp
<Application.DataTemplates>
    <DataTemplate DataType="{x:Type vm:HomePageViewModel}">
        <views:HomePageView/>
    </DataTemplate>

    <DataTemplate DataType="{x:Type vm:SettingsPageViewModel}">
        <views:SettingsPageView/>
    </DataTemplate>
</Application.DataTemplates>	
```

Мы создаём шаблоны, в рамках которых для конкретного типа данных (ViewModel) создаётся соответствующий View. По сути, мы описываем mapping.

Ценность данного шаблона состоит в том, что не нужно управлять привязками IsVisible вручную. Каждый View связан только со своим ModelView, что позволяет избежать создания огромных перегруженных, разделяемых между разными Views общих ViewModel.

## На что нужно обратить внимание при разработке кода

Если мы создаём ViewModal через генерацию кода, необходимо добавить наследование от **ViewModelBase**:

```csharp
internal class HomePageViewModel : ViewModelBase
```

## Спец.эффекты

При переключении между страницами можно включить эффект визуальной трансформации. Для этого следует изменить файл "MainWindow.axaml" заменив вот этот код:

```csharp
<ContentControl Content="{Binding CurrentPageViewModel}" />
```

На описание эффекта визуальной трансформации контента:

```csharp
<TransitioningContentControl Content="{Binding CurrentPageViewModel}">
    <TransitioningContentControl.PageTransition>
        <CrossFade Duration="0:0:0.25" />
    </TransitioningContentControl.PageTransition>
</TransitioningContentControl>
```
