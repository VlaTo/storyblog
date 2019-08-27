namespace LibraProgramming.Windows.UI.Xaml.Core.Markups
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class MarkupVisitor
    {
        public abstract void Visit(MarkupNode node);
    }
}