using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Weavy.Core.Attributes;
using Weavy.Core.Models;

namespace Wvy.Models
{
    [Serializable]
    [Guid("7e5fb812-71c7-4e1e-be0d-567e06f6dae9")]
    [App(Icon = "application", Name = "Kanban", Description = "Track your tasks here", AllowMultiple = true)]
    public class KanbanApp : App
    {
    }
}