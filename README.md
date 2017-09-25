# STpcInfo2
## Overview
This project was designed to show real-time system information on a ssd1306 display using a STM32f4 board and and a UART-USB converter.
## Description
The project uses STM32 HAL libraries to interact with the hardware on the STM32f4 board, the ssd1306 display and the UART-USB converter. A C# application is responsible for sending current information about the system. The project also makes use of Virtual COM Port.
## Tools
- System Workbench for STM32
- STM32 ST-LINK Utility
- STM32CubeMX
- STSW-STM32102 - STM32 Virtual COM Port Driver
- Visual Studio 2012
## C# Application
This application is using System.Management and System.Diagnostics libraries to get current information about the system as:
- current clock speed
- core temperature
- fan speed
- battery status
- available RAM
- and more.
Then sends the data as string through destined serial port.
## How to run
1. Connnect ssd1306 display and UART-USB converter as shown in the following pins scheme
- PA10 - USART1_RX
- PA9 - USART1_TX
- PB1 - DC
- PC5 - CS
- PA7 - SPI_MOSI
- PA5 - SPI_SCK
2. Compile and downlaod the program to your STM32f4 board.
3. Run the C# application located in folder VS2012 as Administrator.
4. Select one of the avalaible COM Ports displayed on screen.
## How to compile
1. Clone this repository
2. Import project into System Workbench for STM32
3. Build All
4. Download the program to your STM32f4
## Future Improvements
The project contains a major bug that results in malformed data being sent over UART.
## License 
https://github.com/PUT-PTM/STpcInfo2/LICENSE.md


The project was conducted during the Microprocessor Lab course held by the Institute of Control and Information Engineering, Poznan University of Technology. Supervisor: Tomasz Mańkowski
