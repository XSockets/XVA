##XVA-02-01 Move Shape

### Installed Nuget Packages

- XSockets
- XSockets.JsApi
- jQuery & jQueryUI

### Description

This sample shows that we can send data between clients at high frequency.

When you move/drag the square in one window all other windows will get data about the position of the shape and move it instantly.

Do note that you will see some slow movements at startup when using Owin (win8+). You will not see that when using XSockets (Any OS).

/Team XSockets