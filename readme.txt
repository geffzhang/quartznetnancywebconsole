Quartz.Net web console is a simple drop-in web-console to manage Quartz.Net in windows services.

By default, console is available on http://localhost:1234/quartzconsole

To configure and start console, please use following code:

	void Start()
	{
		_nancyHost = QuartzConsoleStarter.Configure
			.UsingSchedulerFactory(factory)
			.HostedOnDefault() // or .HostedOn("http://localhost:1234/")
			.IgnoreInScheduleView("list", "of", "ignored", "keys", "in", "schedule", "view")
			.Start();
	}

	void Stop()
	{
		if(_nancyHost != null)
			_nancyHost.Stop();
	}

If you wish to override default url binding, just specify a new one using appSettings 'quartznet.webconsole.host' property.

Unfortunately, during installation of Razor package, some unnececary sections may be created in app.config. 
They are not nececary, feel free to remove them.

Happy scheduling!