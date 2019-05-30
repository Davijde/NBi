﻿using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace NBi.Core.WindowsService
{
	class WindowsServiceCondition : IDecorationCondition
	{
		public static WindowsServiceCondition IsRunning(IWindowsServiceConditionMetadata check)
		{
			var chk = new WindowsServiceCondition(check.ServiceName, check.TimeOut);
			chk.Predicate = chk.IsRunning;
			chk.Message = string.Format("Check that the service named '{0}' is running.", check.ServiceName);
			return chk;
		}

		protected WindowsServiceCondition(string serviceName, int timeout)
		{
			ServiceName=serviceName;
			Timeout = timeout;
		}

		public string ServiceName { get; set; }
		public string Message { get; set; }
		public int Timeout { get; set; }
		protected Func<bool> Predicate { get; set; }

		public bool Validate()
		{
			return Predicate.Invoke();
		}

		protected bool IsRunning()
		{
            if (!IsExisting())
                return false;

            var service = new ServiceController(ServiceName);

			//If current status is starting then wait for X milliseconds and then execute the check.
			if (service.Status == ServiceControllerStatus.StartPending)
			{
				var timeout = TimeSpan.FromMilliseconds(Timeout);
				service.WaitForStatus(ServiceControllerStatus.Running, timeout);
			}
			return service.Status == ServiceControllerStatus.Running;
		}

        protected bool IsExisting()
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(ServiceName));
        }

    }
}
