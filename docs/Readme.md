# Dotnet logger wrapper
Dotnet logger wrapper for fastest usage

| Environment   | Build         | Code Quality | Release | 
| -----------   | -----------   |--------------|-----------
| Production    | [![Build Status](https://dev.azure.com/CLiCS0832/DotnetCoreLogger/_apis/build/status/BrainStation-23.AspNetCoreLogger?branchName=master)](https://dev.azure.com/CLiCS0832/DotnetCoreLogger/_build/latest?definitionId=13&branchName=master) | [![Quality Gate Status](http://52.255.157.110:9000/api/project_badges/measure?project=DotnetCoreLogger&metric=alert_status&token=e6a362d8ded3f255599c8c0ebad252eed46f548e)](http://52.255.157.110:9000/dashboard?id=DotnetCoreLogger) | [![Production Release Status](https://vsrm.dev.azure.com/CLiCS0832/_apis/public/Release/badge/4f3d3ec5-6d43-4f46-b1fd-7b8d37a4cceb/1/2)](https://dev.azure.com/CLiCS0832/DotnetCoreLogger/_release?definitionId=1&view=mine&_a=releases)
| Develop       | [![Build Status](https://dev.azure.com/CLiCS0832/DotnetCoreLogger/_apis/build/status/BrainStation-23.AspNetCoreLogger?branchName=develop)](https://dev.azure.com/CLiCS0832/DotnetCoreLogger/_build/latest?definitionId=13&branchName=develop) | - |  [![Develop Release Status](https://vsrm.dev.azure.com/CLiCS0832/_apis/public/Release/badge/4f3d3ec5-6d43-4f46-b1fd-7b8d37a4cceb/1/1)](https://dev.azure.com/CLiCS0832/DotnetCoreLogger/_release?definitionId=1&view=mine&_a=releases)


## Why logger wrapper
- Dotnet has default logger, but has some limitations
  - too much nosiy
  - no persistant providers
  - no cloud providers
- need fasted usage
- need some builtin configure for plug & play
- need some additional data
- need some additional logger eg. activity, request with more data, error, audit trail etc.

** Our main goal is to create a wrapper with existing/non exisiting libraries for fastest usage in  new projects.

## Logger wrapper includes
- Request logs
- Error logs
- DB Audit trail logs
- Notification logs
- Activity logs
- DB Queies logs

## References
[Usage](usage.md)  
[Status](status.md) 



