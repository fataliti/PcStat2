# PcStat2

Маленький экранчик на esp8266 для отображения нагрузок и температур вашего ПК 
![display.jpg](https://github.com/fataliti/PcStat2/blob/main/monitor.jpg)
Отображает текущую нагруженность ЦП, ГПУ, ОЗУ и температуры ЦП и ГПУ

Чтобы воспользоаться, нужно просто скачать и подставить свои значения в коде, запустить с правами админа (для получения некоторых значений они нужны, иначе выдает 0)

Понадобится еще воспользоваться сервисом [hivemq](https://www.hivemq.com/)

Тулза работает в фоне, без окон и отображения в трее, можно поставить в автозапуск, тогда она будет работать сразу же как включится пк

## Работает так:
- на пк используется тулза написанная на c# используюащя OpenHardwareMonitor, которая собирает значения и отправляет их через mqtt сервис
- esp8266 получает значения и отображает их на экранчике который приклеен к вашему монитру на 2х сторонний скотч



# Eng version
A small screen on esp8266 to display the loads and temperatures of your PC. It shows the current CPU, GPU, RAM loads, and CPU and GPU temperatures.
To use it, simply download and substitute your values in the code, then run it with administrator privileges (some values require them, otherwise it returns 0).

You'll also need to use the [hivemq](https://www.hivemq.com/) service.

The tool runs in the background without windows or tray display. You can set it to start automatically, and it will work as soon as your PC is turned on.

How it works:
A tool written in C# using OpenHardwareMonitor is used on the PC. It collects values and sends them through the MQTT service.
Esp8266 receives the values and displays them on the screen attached to your monitor with double-sided tape.
