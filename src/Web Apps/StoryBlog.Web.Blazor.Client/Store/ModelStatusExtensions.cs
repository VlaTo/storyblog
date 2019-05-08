namespace StoryBlog.Web.Blazor.Client.Store
{
    public static class ModelStatusExtensions
    {
        public static bool IsNone(this IHasModelStatus model) =>
            null != model && ModelStatus.None == model.Status;

        public static bool IsFailed(this IHasModelStatus model) => 
            null == model || ModelState.Error == model.Status.State;

        public static bool IsLoading(this IHasModelStatus model) =>
            null != model && ModelStatus.Loading == model.Status;
    }
}