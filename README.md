ArtemisBridgeMonitor
================
A simple GUI and extensibility interface to allow you to monitor active games of Artemis SBS <http://www.artemis.eochu.com/>.

This allows you to write .Net plugins that can listen for events on a specific player ship, and fire off code without having to resort to the DMX hooks exposed natively by the Artemis executable. Currently, this project relies on the conversion of my ArtClientMonitor <https://github.com/mentalspike/ArtClientMonitor> java code into a .Net dll, however if the rumored extensibility hooks are added directly to the Artemis exe I will do what I can to remove the dependency on that code.

Included in repository is the preliminary code I use for integrating a Rockband Stage Kit light/fog machine into some of the operations of your spaceship.

This entire project is currently very rough and needs work before I can declare it ready for public consumption.