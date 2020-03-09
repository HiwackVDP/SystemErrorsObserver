# [WIP] SystemErrorsObserver
This project aims to be a background service listening to windows event log and creating windows push 
notifications for each error logged.

## Limitations
- Currently only listens to the ``Application`` and ``System`` event logs
- There is currently no way to practically stop the service, the only way is to kill the process.
