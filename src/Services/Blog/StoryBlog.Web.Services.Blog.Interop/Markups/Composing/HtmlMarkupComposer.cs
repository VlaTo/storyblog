using StoryBlog.Web.Services.Blog.Interop.Markups.Nodes;
using StoryBlog.Web.Services.Blog.Interop.Markups.Parsing;
using System;
using System.Text;

namespace StoryBlog.Web.Services.Blog.Interop.Markups.Composing
{
    public sealed class HtmlMarkupComposer : BulletingBoardDocumentVisitor, IDisposable
    {
        private HtmlTagWriter writer;
        private bool disposed;

        public HtmlMarkupComposer(StringBuilder stringBuilder)
        {
            writer = new HtmlTagWriter(stringBuilder);
        }
        
        public override void Visit(BulletingBoardDocument obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            base.Visit(obj);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected override void RenderBold(BulletingBoardBold block)
        {
            using (writer.OpenTag(block.BlockType))
            {
                base.RenderBold(block);
            }
        }

        protected override void RenderUnderline(BulletingBoardUnderline block)
        {
            using (writer.OpenTag(block.BlockType))
            {
                base.RenderUnderline(block);
            }
        }

        protected override void RenderHyperlink(BulletingBoardHyperlink link)
        {
            using (writer.OpenTag(link.BlockType, link.Argument))
            {
                base.RenderHyperlink(link);
            }
        }

        protected override void RenderText(BulletingBoardText inline)
        {
            writer.WriteText(inline.Text);
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    writer.Dispose();
                    writer = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}