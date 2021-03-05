using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace UIUtils
{
	public class BulkObservableCollection<T> : ObservableCollection<T>
	{
		private bool suppressed;

		public BulkObservableCollection() : base()
		{
			this.suppressed = false;
		}

		public BulkObservableCollection(IEnumerable<T> source) : base(source)
		{
			this.suppressed = false;
		}

		public BulkObservableCollection(List<T> source) : base(source)
		{
			this.suppressed = false;
		}

		public void AddRange(IEnumerable<T> source)
		{
			using (this.SuppressiNotifications())
			{
				foreach (var value in source)
				{
					base.Add(value);
				}
			}
		}

		public void RemoveRange(IEnumerable<T> source)
		{
			using (this.SuppressiNotifications())
			{
				foreach (var value in source)
				{
					base.Remove(value);
				}
			}
		}

		public IDisposable SuppressiNotifications()
		{
			return new SuppressionDisposer(this);
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (!suppressed)
			{
				base.OnCollectionChanged(e);
			}
		}


		internal class SuppressionDisposer : IDisposable
		{
			private readonly BulkObservableCollection<T> source;

			public SuppressionDisposer(BulkObservableCollection<T> source)
			{
				if (source.suppressed)
				{
					return;
				}
				this.source = source;
				this.source.suppressed = true;
			}

			public void Dispose()
			{
				if(this.source == null)
				{
					return;
				}

				this.source.suppressed = false;
				this.source.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}
	}
}
