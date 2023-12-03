#include <ESP8266WiFi.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <PubSubClient.h>
#include <ArduinoJson.h>

#define SCREEN_WIDTH 128 
#define SCREEN_HEIGHT 32 
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, -1);

const char* wifi_ssid = "";
const char* wifi_pass = "";

const char* mqtt_server = "hivemq.cloud";
const int mqtt_port = 8883;
const char* mqrtt_user = "";
const char* mqrtt_pass = "";

WiFiClientSecure esp_client;
PubSubClient mqtt_client(esp_client);

int shift_x = 4;
int shift_y = -7;

unsigned long last_update_time;
bool is_active = false;

void DrawText(int _x, int _y, String _text) {
  display.setCursor(_x, _y);
  display.println(_text);
  display.drawRect(_x - 3, _y -3, 30, 13, WHITE);
}

void mqtt_callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message get in topic: ");
  Serial.println(topic);

  char json[length + 1];
  memcpy(json, payload, length);
  json[length] = '\0';

  StaticJsonDocument<1024> doc;
  deserializeJson(doc, json);

  double CpuTemp  = doc["CpuTemp"]; 
  double GpuTemp  = doc["GpuTemp"]; 
  double CpuLoad  = doc["CpuLoad"]; 
  double GpuLoad  = doc["GpuLoad"];  
  double MemLoad  = doc["MemLoad"];  

  display.clearDisplay();

  DrawText(shift_x + 0, shift_y + 10, String(CpuLoad, 0) + "%");
  DrawText(shift_x + 45, shift_y + 10, String(GpuLoad, 0) + "%");
  DrawText(shift_x + 90, shift_y + 10, String(MemLoad, 0) + "%");

  DrawText(shift_x + 20, shift_y + 28, String(CpuTemp, 0) + "C");
  DrawText(shift_x + 70, shift_y + 28, String(GpuTemp, 0) + "C");

  display.display();

  last_update_time = millis();
  is_active = true;
}

  void wifiConnect() {
    WiFi.begin(wifi_ssid, wifi_pass);
    while (WiFi.status() != WL_CONNECTED) {
      delay(1000);
      Serial.println("Подключение к Wi-Fi...");
    }
    Serial.println("Подключение к Wi-Fi выполнено");
  }
  
  void mqttConnect() {
    mqtt_client.setServer(mqtt_server, mqtt_port);
    mqtt_client.setCallback(mqtt_callback);
    Serial.println("mqtt connecting");
    esp_client.setInsecure();

    while (!mqtt_client.connect("ESP8266Client", mqrtt_user, mqrtt_pass)) {
      delay(1000);
      Serial.println("connecting");
    }
    Serial.println("mqtt connected");
    mqtt_client.subscribe("hw_monitor");
  }

void setup() {
  Serial.begin(115200);

  if(!display.begin(SSD1306_SWITCHCAPVCC, 0x3C)) { // Address 0x3D for 128x64
    Serial.println(F("SSD1306 allocation failed"));
    for(;;);
  }
  delay(2000);
  display.clearDisplay();

  display.setTextColor(WHITE);
  display.setFont();
  display.setTextSize(1);
  display.setRotation(2);

  display.setCursor(40, 15);
  display.println("LOADING");
  display.display(); 

  wifiConnect();
  mqttConnect();

  display.clearDisplay();
  display.setCursor(40, 15);
  display.println("CONNECTED");
  display.display(); 
}

void loop() {
  mqtt_client.loop();

  if (is_active) {
    if (((millis() - last_update_time) / 1000) > 10) {
      is_active = false;
      display.clearDisplay();
      display.display(); 
    }
  }
}

