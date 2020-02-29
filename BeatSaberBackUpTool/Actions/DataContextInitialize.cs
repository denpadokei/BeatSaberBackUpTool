using BeatSaberBackUpTool.Interfaces;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeatSaberBackUpTool.Actions
{
    public class DataContextInitialize : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject.DataContext is IInitializable context) {
                context.Initialize();
            }
            
            //throw new NotImplementedException();
        }
    }
}
