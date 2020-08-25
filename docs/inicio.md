# ¿Qué es?

En este documento se exponen los pasos a seguir para poder llegar al punto en el que poder ponerte a desarrollar módulos en C# para FiveM en Windows 10.

# Pasos

Por supuesto, tener comprado e instalado el GTA V, ya que cuando instalemos FiveM, nos pedirá la ruta de instalación del GTA V.

## Instalación de Microsoft Visual Studio Community 2019

Este entorno nos aportará las herramientas para poder desarrollar en C#. La página web desde la que se puede descargar el software es <a href="https://visualstudio.microsoft.com/es/vs/">ésta</a>, donde seleccionaremos `Community 2019` en el desplegable:

<img src="img/web.jpg">

Terminada la descarga, procedemos a su instalación, donde deberemos seleccionar la opción `Desarrollo de escritorio de .NET` y avanzar con la instalación:

<img src="img/ainstalar.jpg">

Cuando termine, continuar con el siguiente apartado. Más adelante indicaremos los pasos a seguir para crear un nuevo proyecto

## Instalación de Git

Nos hará falta para clonar repositorios e incluso, cuando desarrollemos, para poder compartir nuestro código con el mundo. Para ello vamos <a href="https://git-scm.com/download/win">aquí</a> y lo descargamos:

<img src="img/webgit.jpg">

 La instalación es sencilla y pueden dejarse los parámetros por defecto, al menos que se desee modificar alguno.

## Instalación de FiveM

Para poder desarrollar y probar los módulos vamos a necesitar FiveM, el cual podemos descargar de <a href="https://fivem.net/">aquí</a>:

<img src="img/webfivem.jpg">

FiveM es el que nos va a proporcionar las librerías para poder llevar a cabo el desarrollo.

## Pasos a seguir para la ejecución del server

1. Creación de un nuevo directorio en el que descargaremos lo necesario para poder ejecutar el servidor. Por convención, podemos crear lo siguiente: `./FXServer/server`
2. Descargamos la última build de la rama `master` de <a href="https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/">aquí</a>. Tener en cuenta que es la versión para Windows.
3. El contenido del `.zip` lo extraemos en la carpeta `server` creada anteriormente.
4. Clonamos <a href="https://github.com/citizenfx/cfx-server-data">este</a> repositorio en la carpeta `FXServer`, creada anteriormente. Estando en `FXServer`, podemos usar el comando:

```bat
git clone https://github.com/citizenfx/cfx-server-data.git server-data
```

5. En la carpeta `server-data` creada, creamos un archivo `server.cfg` y lo llenamos con el contenido de ejemplo de <a href="https://docs.fivem.net/docs/server-manual/setting-up-a-server/#servercfgexample">aquí</a>. Aún así, en el [anexo](#a1) de este documento tenemos también dicho contenido.
6. Obtenemos una clave de licencia para poder desarrollar <a href="https://keymaster.fivem.net/">aquí</a>
7. La añadimos al archivo `server.cfg`, creado anteriormente, junto a la llave `sv_licenseKey aquieberiaIr`
8. Creamos un archivo nuevo, como por ejemplo, `server.bat` y le añadimos lo siguiente:
```bat
cd C:\Users\nombreUsuario\FXServer\server-data
C:\Users\nombreUsuario\FXServer\server\FXServer.exe +exec server.cfg
```

Una vez terminados todos los pasos, podemos ejecutar el `server.bat` y debería abrirse una consola, donde veremos que se cargan ya algunos módulos por defecto. En esta consola será desde donde podremos cargar los módulos que vayamos creando. Si todo ha funcionado correctamente, debería mostrar algo parecido a lo siguiente:

<img src="img/consola.jpg">

Si le damos a `Enter` podremos ver que se puede escribir en la consola. Algunos comandos a recordar son `start nombreModulo`, `stop nombreModulo`, `restart nombreModulo` y `refresh`.

# Iniciar FiveM

Ahora ya podemos ejecutar FiveM. Una vez dentro, debemos hacer clic en el botón de localhost, que automáticamente debería conectarse al recién iniciado servidor:

<img src="img/localhost.jpg">

Mientras cargan todos los elementos necesario, veremos una pantalla de carga en la que podremos entretenernos con el movimiento de un monigote mientras esperamos:

<img src="img/localhost1.jpg">

Una vez cargado todo, nos hará aparecer en un lugar aletorio del mapa. Entonces ya podemos cerciorarnos de que ha funcionado correctamente. Si queremos acceder a la consola dentro del juego usaremos `F8`. Para acceder al chat usaremos la letra `T`.

# Creación del primer módulo en C# usando MVSC

Cuando iniciemos el MVSC, lo primero que haremos será crear un nuevo proyecto:

<img src="img/nuevoproyecto.jpg">

Tras darle a siguien, buscamos **biblioteca de clases (.NET Framework)** y la escogemos:

<img src="img/bibliotecaclases.jpg">

En la siguiente pantalla indicamos la configuración que deseemos:

<img src="img/nombreproyecto.jpg">

Finalmente, lo creamos. Una vez dentro, hacemos clic con el botón derecho en la clase en el panel derecho, justo debajo de donde pone `Solución nombreClase`, y seleccionamos **Propiedades**:

<img src="img/propiedades.jpg">

En el apartado **aplicación** tenemos que añadirle la extensión `.net` al nombre del ensamblado y guardar con `Control+S`:

<img src="img/net.jpg">

Por último, volvemos a hacer clic derecho en el mismo lugar pero esta vez vamos a **Agregar** y escogemos **Referencia**:

<img src="img/referencia.jpg">

En el apartado **Examinar** tendremos que añadir el `.dll` cuyo nombre es `CitizenFX.Core.dll`. Éste debería encontrarse en la ruta `C:\Users\nombreUsuario\AppData\Local\FiveM\FiveM.app\citizen\clr2\lib\mono\4.5`. Lo copiamos y lo pegamos en el proyecto de MVSC creado. Tras ello lo añadimos desde el apartado **Examinar**. Asegurémonos de que está marcado:

<img src="img/citizenfxcore.jpg">

Una vez hecho esto, ya podemos empezar a programar. En el [anexo](#a2) dejo un código básico de ejemplo, el cual podemos añadir al proyecto. Debería funcionar sin problemas, ya que todas las dependencias están cubiertas. Una vez lo hemos pegado en el archivo creado, seleccionaremos la opción release:

<img src="img/release.jpg">

Y compilaremos la solución:

<img src="img/compile.jpg">

Con esta compilación obtendremos una serie de archivos. El que nos interesa es el que tiene el nombre de nuestra solución, que en mi caso es `ClassLibrary1.net.dll`:
 
 <img src="img/compiled.jpg">

Copiamos dicho `.dll` y lo pegamos en `rutaEscogida\server\server-data\resources\[local]\MiPrimerModulo`, siendo **MiPrimerModulo** una carpeta que creamos, preferiblemente, con el nombre del propio módulo:

<img src="img/classlibrary.jpg">

Tras esto tenemos que crear un `fxmanifest.lua` con el contenido que indicamos en el [anexo](#a3). Lo modificamos según nuestras necesidades:

<img src="img/fxmanifest.jpg">

Ahora, desde la consola, hacemos un `refresh` para que detecte el nuevo módulo añadido:

<img src="img/refreshmodule.jpg">

Tras ello, iniciamos el módulo:

<img src="img/startmodule.jpg">

Si ahora volvemos a FiveM, veremos desde la consola de desarrollador que se ha cargado correctamente:

<img src="img/loaded.jpg">

Si vamos al chat del juego y escribimos `/car`:

<img src="img/chat.jpg">

Debería hacer aparecer un coche y nos meterá automáticamente dentro de él. Además de mostrarnos por chat el mensaje que le hemos indicado en el código con el nombre del modelo:


<img src="img/coche.jpg">

Tener en cuenta que, por defecto, si no le hemos indicado ningún parámetro, nos hace aparecer un coche por defecto, cuyo modelo es el `adder`. Pero si le pasas el modelo deseado como parámetro, lo hará aparecer:

<img src="img/gp1.jpg">

Si, por otro lado, el modelo no existe, entonces lo indica por chat:

<img src="img/noexiste.jpg">

Si queremos hacer que deje de estar activo el módulo, podemos escribir en la consola `stop nombreModulo`:

<img src="img/stopmodule.jpg">

Y vemos que ahora ya no hace nada el comando en FiveM:

<img src="img/nospawn.jpg">

Pues con esto se termina el tutorial que debería permitiros poder desarrollar sin problemas módulos para FiveM.

# Anexo
## A1
```sh
# Only change the IP if you're using a server with multiple network interfaces, otherwise change the port only.
endpoint_add_tcp "0.0.0.0:30120"
endpoint_add_udp "0.0.0.0:30120"

# These resources will start by default.
ensure mapmanager
ensure chat
ensure spawnmanager
ensure sessionmanager
ensure fivem
ensure hardcap
ensure rconlog
ensure scoreboard

# This allows players to use scripthook-based plugins such as the legacy Lambda Menu.
# Set this to 1 to allow scripthook. Do note that this does _not_ guarantee players won't be able to use external plugins.
sv_scriptHookAllowed 0

# Uncomment this and set a password to enable RCON. Make sure to change the password - it should look like rcon_password "YOURPASSWORD"
#rcon_password ""

# A comma-separated list of tags for your server.
# For example:
# - sets tags "drifting, cars, racing"
# Or:
# - sets tags "roleplay, military, tanks"
sets tags "default"

# A valid locale identifier for your server's primary language.
# For example "en-US", "fr-CA", "nl-NL", "de-DE", "en-GB", "pt-BR"
sets locale "root-AQ" 
# please DO replace root-AQ on the line ABOVE with a real language! :)

# Set an optional server info and connecting banner image url.
# Size doesn't matter, any banner sized image will be fine.
#sets banner_detail "https://url.to/image.png"
#sets banner_connecting "https://url.to/image.png"

# Set your server's hostname
sv_hostname "FXServer, but unconfigured"

# Nested configs!
#exec server_internal.cfg

# Loading a server icon (96x96 PNG file)
#load_server_icon myLogo.png

# convars which can be used in scripts
set temp_convar "hey world!"

# Uncomment this line if you do not want your server to be listed in the server browser.
# Do not edit it if you *do* want your server listed.
#sv_master1 ""

# Add system admins
add_ace group.admin command allow # allow all commands
add_ace group.admin command.quit deny # but don't allow quit
add_principal identifier.fivem:1 group.admin # add the admin to the group

# Hide player endpoints in external log output.
sv_endpointprivacy true

# enable OneSync with default configuration (required for server-side state awareness)
onesync_enabled true

# Server player slot limit (must be between 1 and 32, unless using OneSync)
sv_maxclients 32

# Steam Web API key, if you want to use Steam authentication (https://steamcommunity.com/dev/apikey)
# -> replace "" with the key
set steam_webApiKey ""

# License key for your server (https://keymaster.fivem.net)
sv_licenseKey changeme
```

## A2 

```C#
using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace MyResourceNameClient
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("car", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                // account for the argument not being passed
                var model = "adder";
                if (args.Count > 0)
                {
                    model = args[0].ToString();
                }

                // check if the model actually exists
                // assumes the directive `using static CitizenFX.Core.Native.API;`
                var hash = (uint)GetHashKey(model);
                if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[CarSpawner]", $"It might have been a good thing that you tried to spawn a {model}. Who even wants their spawning to actually ^*succeed?" }
                    });
                    return;
                }

                // create the vehicle
                var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);

                // set the player ped into the vehicle and driver seat
                Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);

                // tell the player
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[CarSpawner]", $"Woohoo! Enjoy your new ^*{model}!" }
                });
            }), false);
        }
    }
}

```

## A3

```lua
fx_version 'bodacious'
game 'gta5'

author 'nombreAutor'
description 'Descrición Módulo'
version '1.0.0'

resource_type 'gametype' { name = 'nombreRecurso' }

client_scripts {'*.dll'}
```