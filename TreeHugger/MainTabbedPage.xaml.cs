namespace TreeHugger;

public partial class MainTabbedPage : TabbedPage
{
	public MainTabbedPage()
	{
		InitializeComponent();
        CurrentPage = Children[1]; // Display the map page first
    }
}
