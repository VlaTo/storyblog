﻿using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Client.Components;
using System;

namespace StoryBlog.Web.Client.Core
{
    public sealed class ModalContent : IModalContent
    {
        public string Title
        {
            get;
        }

        public RenderFragment Content
        {
            get;
        }

        public ModalButton[] Buttons
        {
            get;
        }

        public ModalContent(string title, Type contentType, params ModalButton[] buttons)
        {
            Title = title;
            Buttons = buttons;
            Content = builder =>
            {
                builder.OpenComponent(1, contentType);
                builder.CloseComponent();
            };
        }
    }
}