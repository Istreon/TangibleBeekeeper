#include <ESP8266WiFi.h> 

extern "C" {
#include "user_interface.h"
#include "c_types.h"
}

#define bouton 2                                         // affectation des broches                                     
boolean etat;                                               // déclaration de la variable etat de type booleén

const char* ssid = "ENTREZ_NOM_RESEAU_ICI";
const char* password = "ENTREZ_MDP_ICI";
//int ledPin = 2;



// ATTENTION GPIO13 correspond à la PIN 07
WiFiServer server(80);

void setup() {
  Serial.begin(9600);
  delay(10);
  //pinMode(ledPin, OUTPUT);
  //digitalWrite(ledPin, LOW);

  pinMode(bouton, INPUT_PULLUP);         // la broche bouton est en entrée avec une résistance de tirage
  pinMode(13, OUTPUT);  
 
 // Connect to WiFi network
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
 
  // Start the server
  server.begin();
  Serial.println("Server started");
 
  // Print the IP address
  Serial.print("Use this URL to connect: ");
  Serial.print("http://");
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
