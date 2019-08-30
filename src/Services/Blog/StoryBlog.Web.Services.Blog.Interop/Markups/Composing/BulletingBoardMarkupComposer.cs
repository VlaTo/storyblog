using StoryBlog.Web.Services.Blog.Interop.Markups.Nodes;
using StoryBlog.Web.Services.Blog.Interop.Markups.Parsing;
using System;
using System.Text;

namespace StoryBlog.Web.Services.Blog.Interop.Markups.Composing
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BulletingBoardMarkupComposer : BulletingBoardDocumentVisitor
    {
        private readonly BulletingBoardTagWriter writer;

        public BulletingBoardMarkupComposer(StringBuilder stringBuilder)
        {
            writer = new BulletingBoardTagWriter(stringBuilder);
        }

        public override void Visit(BulletingBoardDocument obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            base.Visit(obj);
        }

        protected override void RenderStrong(BulletingBoardStrong block)
        {
            using (writer.OpenTag(block.BlockType))
            {
                base.RenderStrong(block);
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
    }
}