using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace lab07.Controls;

public class SearchResultItem
{
    public string Name    { get; set; } = string.Empty;
    public string Snippet { get; set; } = string.Empty;

    public Visibility SnippetVisibility => string.IsNullOrEmpty(Snippet)
        ? Visibility.Collapsed
        : Visibility.Visible;
}

public class SearchPanelItemSelectedEventArgs : RoutedEventArgs
{
    public string NoteName { get; }

    public SearchPanelItemSelectedEventArgs(RoutedEvent routedEvent, string noteName)
        : base(routedEvent)
    {
        NoteName = noteName;
    }
}

public class SearchPanelPreviewEventArgs : RoutedEventArgs
{
    public string Query  { get; }
    public bool   Cancel { get; set; } = false;

    public SearchPanelPreviewEventArgs(RoutedEvent routedEvent, string query)
        : base(routedEvent)
    {
        Query = query;
    }
}

public class SearchPanelDirectEventArgs : RoutedEventArgs
{
    public int ResultCount { get; }

    public SearchPanelDirectEventArgs(RoutedEvent routedEvent, int count)
        : base(routedEvent)
    {
        ResultCount = count;
    }
}

public partial class SearchPanel : UserControl, INotifyPropertyChanged
{
    // ─── RoutedEvent: ItemSelected — Bubbling ─────────────────────────────────
    // Всплывает от SearchPanel вверх. MainWindow обрабатывает его и открывает заметку.

    public static readonly RoutedEvent ItemSelectedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(ItemSelected),
            RoutingStrategy.Bubble,
            typeof(EventHandler<SearchPanelItemSelectedEventArgs>),
            typeof(SearchPanel));

    public event EventHandler<SearchPanelItemSelectedEventArgs> ItemSelected
    {
        add    => AddHandler(ItemSelectedEvent, value);
        remove => RemoveHandler(ItemSelectedEvent, value);
    }

    // ─── RoutedEvent: PreviewSearch — Tunneling ───────────────────────────────
    // Идёт сверху вниз к SearchPanel. Родитель может отменить поиск до его запуска.

    public static readonly RoutedEvent PreviewSearchEvent =
        EventManager.RegisterRoutedEvent(
            nameof(PreviewSearch),
            RoutingStrategy.Tunnel,
            typeof(EventHandler<SearchPanelPreviewEventArgs>),
            typeof(SearchPanel));

    public event EventHandler<SearchPanelPreviewEventArgs> PreviewSearch
    {
        add    => AddHandler(PreviewSearchEvent, value);
        remove => RemoveHandler(PreviewSearchEvent, value);
    }

    // ─── RoutedEvent: SearchCompleted — Direct ────────────────────────────────
    // Не всплывает и не проваливается. Только сам SearchPanel получает это событие.
    // Используется для локальной реакции на завершение поиска (например, обновление счётчика).

    public static readonly RoutedEvent SearchCompletedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(SearchCompleted),
            RoutingStrategy.Direct,
            typeof(EventHandler<SearchPanelDirectEventArgs>),
            typeof(SearchPanel));

    public event EventHandler<SearchPanelDirectEventArgs> SearchCompleted
    {
        add    => AddHandler(SearchCompletedEvent, value);
        remove => RemoveHandler(SearchCompletedEvent, value);
    }

    // ─── DependencyProperty: MaxResults ───────────────────────────────────────
    // ValidateValueCallback: значение от 1 до 500
    // CoerceValueCallback:   если дерево не задано — ограничиваем до 10

    public static readonly DependencyProperty MaxResultsProperty =
        DependencyProperty.Register(
            nameof(MaxResults),
            typeof(int),
            typeof(SearchPanel),
            new PropertyMetadata(100, OnMaxResultsChanged, CoerceMaxResults),
            ValidateMaxResults);

    private static bool ValidateMaxResults(object value)
    {
        int n = (int)value;
        return n >= 1 && n <= 500;
    }

    private static object CoerceMaxResults(DependencyObject d, object baseValue)
    {
        var panel = (SearchPanel)d;
        int n     = (int)baseValue;
        if (panel._tree == null)
            return Math.Min(n, 10);
        return n;
    }

    private static void OnMaxResultsChanged(
        DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (SearchPanel)d;
        panel.RunSearch(panel.SearchBox.Text);
    }

    public int MaxResults
    {
        get => (int)GetValue(MaxResultsProperty);
        set => SetValue(MaxResultsProperty, value);
    }

    // ─── DependencyProperty: SnippetLength ────────────────────────────────────
    // ValidateValueCallback: длина сниппета от 20 до 300 символов
    // CoerceValueCallback:   округляем до кратного 10

    public static readonly DependencyProperty SnippetLengthProperty =
        DependencyProperty.Register(
            nameof(SnippetLength),
            typeof(int),
            typeof(SearchPanel),
            new PropertyMetadata(80, OnSnippetLengthChanged, CoerceSnippetLength),
            ValidateSnippetLength);

    private static bool ValidateSnippetLength(object value)
    {
        int n = (int)value;
        return n >= 20 && n <= 300;
    }

    private static object CoerceSnippetLength(DependencyObject d, object baseValue)
    {
        int n = (int)baseValue;
        return (int)(Math.Round(n / 10.0) * 10);
    }

    private static void OnSnippetLengthChanged(
        DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var panel = (SearchPanel)d;
        panel.RunSearch(panel.SearchBox.Text);
    }

    public int SnippetLength
    {
        get => (int)GetValue(SnippetLengthProperty);
        set => SetValue(SnippetLengthProperty, value);
    }

    // ─── Internals ────────────────────────────────────────────────────────────

    private FileSystemTree? _tree;

    private string _queryInfo = string.Empty;
    public string QueryInfo
    {
        get => _queryInfo;
        private set { _queryInfo = value; OnPropertyChanged(); }
    }

    private Visibility _queryInfoVisibility = Visibility.Collapsed;
    public Visibility QueryInfoVisibility
    {
        get => _queryInfoVisibility;
        private set { _queryInfoVisibility = value; OnPropertyChanged(); }
    }

    public SearchPanel()
    {
        InitializeComponent();

        SearchCompleted += (_, args) =>
        {
            System.Diagnostics.Debug.WriteLine(
                $"[SearchPanel] SearchCompleted (Direct): {args.ResultCount} results");
        };
    }

    public void SetTree(FileSystemTree tree)
    {
        _tree = tree;
        CoerceValue(MaxResultsProperty);
        RunSearch(SearchBox.Text);
    }

    private void RunSearch(string query)
    {
        if (_tree == null) return;

        var trimmed = query.Trim();

        var previewArgs = new SearchPanelPreviewEventArgs(PreviewSearchEvent, trimmed)
        {
            Source = this
        };
        RaiseEvent(previewArgs);

        if (previewArgs.Cancel) return;

        var results = new List<SearchResultItem>();

        _tree.Traverse((node, _) =>
        {
            if (node.IsFolder) return;
            if (results.Count >= MaxResults) return;

            if (string.IsNullOrEmpty(trimmed))
            {
                results.Add(new SearchResultItem { Name = node.Name });
                return;
            }

            bool   nameMatch    = node.Name.Contains(trimmed, StringComparison.OrdinalIgnoreCase);
            string snippet      = BuildSnippet(node.Content, trimmed);
            bool   contentMatch = snippet.Length > 0;

            if (nameMatch || contentMatch)
                results.Add(new SearchResultItem { Name = node.Name, Snippet = snippet });
        });

        ResultsList.ItemsSource = results;

        if (string.IsNullOrEmpty(trimmed))
        {
            QueryInfoVisibility = Visibility.Collapsed;
        }
        else
        {
            QueryInfo           = $"Найдено: {results.Count}";
            QueryInfoVisibility = Visibility.Visible;
        }

        RaiseEvent(new SearchPanelDirectEventArgs(SearchCompletedEvent, results.Count)
        {
            Source = this
        });
    }

    private string BuildSnippet(string? content, string query)
    {
        if (string.IsNullOrEmpty(content)) return string.Empty;

        int idx = content.IndexOf(query, StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return string.Empty;

        int end   = Math.Min(content.Length, idx + query.Length + SnippetLength);
        var slice = content[idx..end].ReplaceLineEndings(" ");

        if (idx > 0)              slice = "…" + slice;
        if (end < content.Length) slice += "…";

        return slice;
    }

    private void ResultRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not Border border) return;
        if (border.DataContext is not SearchResultItem item) return;

        RaiseEvent(new SearchPanelItemSelectedEventArgs(ItemSelectedEvent, item.Name)
        {
            Source = this
        });

        e.Handled = true;
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderText.Visibility = string.IsNullOrEmpty(SearchBox.Text)
            ? Visibility.Visible
            : Visibility.Collapsed;

        RunSearch(SearchBox.Text);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}