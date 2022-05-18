#include <Wire.h>
#include <I2C_LCD.h>

I2C_LCD LCD;
uint8_t I2C_LCD_ADDRESS = 0x51; //Device address configuration, the default value is 0x51.

String rx = "";

void setup() {
  Serial.begin(9600);
  Wire.begin();
  Serial1.begin(9600);
}

void loop() {
  LCD.FontModeConf(Font_6x8, FM_ANL_AAA, BLACK_BAC);
  LCD.CharGotoXY(0,0);       //Set the start coordinate.
  if (Serial1.available()) {
    char rec = (char)Serial1.read();
    if(rec != '_'){
      rx.concat(String(rec));
    }
    else
    {
      Serial.println(rx);
      LCD.CleanAll(WHITE);
      LCD.print(rx);
      delay(2000);
      rx = "";
    }
  }
}
