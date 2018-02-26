/* 
 PureMVC C# Port by Andy Adamczak <andy.adamczak@puremvc.org>, et al.
 PureMVC - Copyright(c) 2006-08 Futurescale, Inc., Some rights reserved. 
 Your reuse is governed by the Creative Commons Attribution 3.0 License 
*/

using System;
using System.Collections.Generic;

using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace PureMVC.Patterns
{
    public class Mediator : Notifier, IMediator, INotifier
	{
        public Mediator(string name)
        {
			m_mediatorName = name;
		}

		List<string> notificationList = new List<string> ();
		public IList<string> ListNotificationInterests()
		{
			return notificationList;
		}

		//可以优化，不用每次都循环判断，这里先保证正确性
		public void addNotification (string type)
		{
			int len = notificationList.Count;
			for (int i = 0; i < len; ++i) {
				if (notificationList [i].Equals (type))
					return;				
			}
			notificationList.Add (type);
		}

//		public void removeNotification (string type)
//		{
//			notificationList.Remove (type);
//		}

		public virtual void HandleNotification(INotification notification)
		{
		}
			
		public virtual void OnRegister()
		{
		}
		public virtual void onUpdate()
		{
		}
		public virtual void onEnter()
		{
		}
		public virtual void onExit ()
		{
		}
		public virtual void OnRemove()
		{
		}

		public virtual string MediatorName
		{
			get { return m_mediatorName; }
		}

		public virtual object ViewComponent
		{
			get { return m_viewComponent; }
			set { m_viewComponent = value; }
		}

        protected string m_mediatorName;
        protected object m_viewComponent;
	}
}
