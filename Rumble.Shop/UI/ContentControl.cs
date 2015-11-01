using System;
using Xamarin.Forms;
using Rumble.Shop.UI;

namespace Rumble.Shop.UI
{
	public class ContentControl : ContentView
	{
		public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(ContentControl), null, propertyChanged: OnDataTemplateSelectorChanged);
	
		private DataTemplateSelector currentItemSelector;
		private static void OnDataTemplateSelectorChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((ContentControl)bindable).OnDataTemplateSelectorChanged((DataTemplateSelector)oldvalue, (DataTemplateSelector)newvalue);
		}            
		protected virtual void OnDataTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
		{
			currentItemSelector = newValue;
			SetContent ();
		}

		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(ContentControl), null, propertyChanged: OnDataTemplateChanged);

		private DataTemplate currentItemTemplate;
		private static void OnDataTemplateChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((ContentControl)bindable).OnDataTemplateChanged((DataTemplate)oldvalue, (DataTemplate)newvalue);
		}            
		protected virtual void OnDataTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
		{
			currentItemTemplate = newValue;
			SetContent ();
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			SetContent ();
		}

		private void SetContent()
		{
			if ((currentItemSelector == null && currentItemTemplate == null) || BindingContext == null) {
				Content = null;
				return;
			}

			var template = currentItemSelector != null ? currentItemSelector.SelectTemplate (BindingContext, this) : 
				currentItemTemplate;
			var content = template != null ? template.CreateContent () : null;
			var view = content is View ? content as View : content is ViewCell ? (content as ViewCell).View : null;
			if(view != null)
				view.BindingContext = BindingContext;
			Content = view;
		}
	}
}

