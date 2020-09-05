# Problemas surgidos
En este documento se exponen problemas surgido y cómo solventarlos

## Dos o más jugadores en nuestro servidor

Para permitir que otras personas puedan conectarse al servidor, tendremos que hacer un forwarding de puertos en el router, de tal manera que cuando lleven a cabo una solicitud para conectarse, el router sepa a qué máquina enviar los datos. Para ello, lo que haremos es acceder al router usando el gateway `192.168.1.1` (puede variar, dependerá tanto de la configuración que haya decido darle el proveedor como de tu propia configuración). Nos pedirá usuario/contraseña, los cuales deberían estar por defecto, al menos que el proveedor o tú mismo los hayas cambiado. Hay páginas web que recopilan los pares usuario/contraseña por defecto de los diferentes dispositivos.

Una vez dentro, vamos al apartado de `forwarding` o `virtual server` (o quizá tiene otro nombre según el dispositivo). Ahí  debemos configurar dos reglas:

* Una que PERMITA escuchar y redireccionar desde la interfaz de INTERNET al PUERTO por defecto de FiveM (30120 o el que hayamos configurado) vía TCP y estableciendo la IP LOCAL de nuestro PC
  
* Una que PERMITA escuchar y redireccionar desde la interfaz de INTERNET al PUERTO por defecto de FiveM (30120 o el que hayamos configurado) vía UDP y estableciendo la IP LOCAL de nuestro PC

<img src="./img/Inkedforward_ports_router_LI.JPG">


## Problemas con los scripts

Hubo un problema con un script que se creó y ésto se debía a que había un campo en el `fxmanifest.lua` que no era necesario tener metido. Por ello había conflicto con otros scripts ya existentes. De ahí que eliminamos la línea que aparece tachada en el siguiente pantallazo:

<img src="./img/Inkedremove_line_resource_type_fxmanifest_LI.JPG">