# Dotnet logger wrapper status
## develop
- Phase 1
	- [x] basic ef core crud so that we log for this executions
	- [x] jwt token so that track user

	- [x] implement default logger & depedency injection
	- [x] request log 
	- [x] DB audit trail log for entities including changes columns
	- [x] global exception handling including same pattern	
	- [ ] compatible with dotnet 5, 6
	- [x] add serilog
	- [x] add serilog wrapper

- Phase - 2
	- [ ] add sinks
	- [ ] add enrichers
	- [ ] add custom event in logger
	- [ ] activiy log
	- [ ] notification logs
	- [ ] routing logs
	- [ ] route tracing for microservice
	- [ ] implement azure cosmosdb daata
	- [ ] implement mongodb persistant data
	- [ ] implement redis for tracking faster
- & so on