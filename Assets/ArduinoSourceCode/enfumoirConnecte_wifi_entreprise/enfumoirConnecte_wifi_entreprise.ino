#include <ESP8266WiFi.h> 

extern "C" {
#include "user_interface.h"
#include "wpa2_enterprise.h"
#include "c_types.h"
}

#define bouton 2                                         // affectation des broches                                     
boolean etat;                                               // déclaration de la variable etat de type booleén

// SSID to connect to
char ssid[] = "ENTREZ_NOM_RESEAU_ICI";
char username[] = "ENTREZ_IDENTIFIANT_ICI";
char identity[] = "ENTREZ_VOTRE_NOM_ICI";
char password[] = "ENTREZ_MDP_ICI";

uint8_t target_esp_mac[6] = {0x24, 0x0a, 0xc4, 0x9a, 0x58, 0x28};




// ATTENTION GPIO13 correspond à la PIN 07
WiFiServer server(80);

void setup() {
  Serial.begin(9600);
  delay(10);
  //pinMode(ledPin, OUTPUT);
  //digitalWrite(ledPin, LOW);

  pinMode(bouton, INPUT_PULLUP);         // la broche bouton est en entrée avec une résistance de tirage
  pinMode(13, OUTPUT);  
 
 
  WiFi.mode(WIFI_STA);
 
  Serial.setDebugOutput(true);
  Serial.printf("SDK version: %s\n", system_get_sdk_version());
  Serial.printf("Free Heap: %4d\n",ESP.getFreeHeap());
  
  // Setting ESP into STATION mode only (no AP mode or dual mode)
  wifi_set_opmode(STATION_MODE);

  struct station_config wifi_config;

  memset(&wifi_config, 0, sizeof(wifi_config));
  strcpy((char*)wifi_config.ssid, ssid);
  strcpy((char*)wifi_config.password, password);

  wifi_station_set_config(&wifi_config);
  wifi_set_macaddr(STATION_IF,target_esp_mac);
  

  wifi_station_set_wpa2_enterprise_auth(1);

  // Clean up to be sure no old data is still inside
  wifi_station_clear_cert_key();
  wifi_station_clear_enterprise_ca_cert();
  wifi_station_clear_enterprise_identity();
  wifi_station_clear_enterprise_username();
  wifi_station_clear_enterprise_password();
  wifi_station_clear_enterprise_new_password();
  
  wifi_station_set_enterprise_identity((uint8*)identity, strlen(identity));
  wifi_station_set_enterprise_username((uint8*)username, strlen(username));
  wifi_station_set_enterprise_password((uint8*)password, strlen((char*)password));

  
  wifi_station_connect();
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.print(".");
  }

  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}
 
void loop() {
  // Control de la connection client
  WiFiClient client = server.available();
  if (!client) {
    return;
  }
  Serial.println("Client connected");
  client.write("test");
 
  while (client.connected()) {
    etat=digitalRead(bouton);                        // la variable etat prend la valeur de l'entrée bouton
   
    if(etat == HIGH) {
      client.println("Off");
      digitalWrite(13, HIGH);  
    } else {
      client.println("On");
      digitalWrite(13, LOW);
    }
    delay(16);
  }
  client.stop();
  Serial.println("Client disconnected");
  Serial.println("");
 
}
