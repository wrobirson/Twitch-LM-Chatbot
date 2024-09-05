# Guía de Instalación de LM Studio y Configuración de ChatBot

Esta guía te ayudará a instalar LM Studio, descargar un modelo de lenguaje y configurar el servidor para usarlo con un bot o cualquier otra aplicación. A continuación, se detallan los pasos necesarios para completar este proceso.

## Requisitos del Sistema

Antes de comenzar, asegúrate de que tu equipo cumpla con los siguientes requisitos mínimos:

- **Sistema Operativo**: Windows 10/11, macOS o Linux (distribuciones basadas en Ubuntu recomendadas).
- **Memoria RAM**: Al menos 16 GB (se recomiendan 32 GB para modelos más grandes).
- **GPU (Opcional)**: Se recomienda una tarjeta gráfica compatible con CUDA (NVIDIA) para mejorar el rendimiento.
- **Espacio en Disco**: Al menos 10 GB libres para la instalación básica y los modelos.

## Instalación de LM Studio

1. Accede al [sitio web oficial de LM Studio](#) y selecciona la versión correspondiente a tu sistema operativo (Windows, macOS o Linux).
2. Descarga el instalador.
3. Ejecuta el archivo descargado y sigue las instrucciones del instalador.
4. Acepta los términos de uso y selecciona el directorio de instalación.
5. Una vez completada la instalación, abre LM Studio desde el menú de inicio (Windows) o desde la carpeta de aplicaciones (macOS).

## Descarga de un Modelo de Lenguaje en LM Studio

1. Abre LM Studio.
2. En la pantalla principal, selecciona la opción de descarga de modelos en el menú lateral.
3. Elige un modelo de acuerdo a tus necesidades: los modelos más grandes ofrecen mayor precisión, pero requieren más recursos.
4. Haz clic en **Descargar**. El proceso puede tardar varios minutos dependiendo del tamaño del modelo y la velocidad de tu conexión.
5. Una vez completada la descarga, el modelo aparecerá como **Descargado** y estará listo para su uso.

## Iniciar el Servidor de LM Studio

1. En la pantalla principal de LM Studio, selecciona la opción **Servidor Local**.
2. Asegúrate de que el modelo descargado esté seleccionado en la configuración del servidor.
3. Haz clic en **Iniciar Servidor**. LM Studio comenzará a cargar el modelo y pondrá en marcha el servidor.
4. Cuando el servidor esté en ejecución, recibirás un mensaje de confirmación indicando que el modelo está listo para recibir solicitudes.

## Crear una Aplicación de Twitch

1. Ve a la [Twitch Developer Console](https://dev.twitch.tv/) e inicia sesión con tu cuenta de Twitch.
2. Haz clic en **Registrar tu aplicación** para crear una nueva.
3. Completa los siguientes campos:
   - **Nombre**: Elige un nombre para tu bot.
   - **Direcciones de redireccionamiento OAuth**: Agrega `https://localhost:5001/auth/twitch/callback`.
   - **Categoría**: Selecciona **Chat Bot**.
   - **Tipo de cliente**: Confidencial.
4. Completa el captcha y haz clic en **Crear**.
5. Una vez creada la aplicación, serás redirigido a la consola de aplicaciones. Haz clic en **Administrar**.
6. Al final de la página, encontrarás el campo **ID de cliente**. Cópialo y guárdalo.
7. Presiona el botón **Nuevo Secreto** para generar un valor de cliente secreto. Cópialo y resérvalo.

## Configuración del Bot

1. Descarga el bot desde [Twitch LM ChatBot by wrobirson](#).
2. Descomprime el archivo descargado.
3. Dentro del directorio descomprimido, abre el archivo `appsettings.Production.json`.
4. Reemplaza los valores de `ClientId` y `ClientSecret` con los que obtuviste al crear la aplicación de Twitch.
5. Guarda y cierra el archivo.
6. Ejecuta el bot abriendo el archivo `TwitchLMChatBot.Server.exe`.
7. Abre tu navegador y escribe `localhost:5001` para acceder al bot.
8. Conecta tu cuenta de Twitch. Verás un mensaje en el chat de Twitch indicando que el bot ha iniciado.
9. Selecciona una personalidad para el bot o crea una nueva.
10. Interactúa con el bot en el chat de Twitch utilizando el comando `!ia`.
