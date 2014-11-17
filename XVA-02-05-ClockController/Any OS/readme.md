##XVA-02-05 Clock Controller

### Installed Nuget Packages

- XSockets
- XSockets.JsApi

### Description

In this sample we create a "longrunning controller". That is a internal controller that clients can't connect to... A singleton!

Such a controller is useful when you want to for example poll some resource for data. Polling is bad and ugly, but sometimes needed and then it is better to haev one resource doing so instead of many.

In this case we only tick the current time out to all clients as a proof of concept.
We use RPC style to broadcast to all clients on a specific controller, but you can also be more sophisticated and use the powerful PublishTo<T> or InvokeTo<T> to target subset of clients based on lambda expressions.

### Video

TBD

/Team XSockets


