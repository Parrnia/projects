#include "HomeSpan.h"
#include "Matter.h"
#include <app/server/OnboardingCodesUtil.h>
#include <credentials/examples/DeviceAttestationCredsExample.h>
#include <credentials/DeviceAttestationCredsProvider.h>

using namespace chip;
using namespace chip::app::Clusters;
using namespace esp_matter;
using namespace esp_matter::endpoint;

// Pin configuration
const int LED_PIN = 23;
const int TOGGLE_BUTTON_PIN = 22;
const int DEBOUNCE_DELAY = 500;
int last_toggle = 0;

const uint32_t CLUSTER_ID = OnOff::Id;
const uint32_t ATTRIBUTE_ID = OnOff::Attributes::OnOff::Id;

uint16_t light_endpoint_id = 0;
attribute_t *attribute_ref;

// Device event callback
static void on_device_event(const chip::DeviceLayer::ChipDeviceEvent *event, intptr_t arg) {
  if (event->Type == chip::DeviceLayer::DeviceEventType::kInternetConnectivityChange) {
    if (event->InternetConnectivityChange.IPv4 == chip::DeviceLayer::ConnectivityChange::kConnectivity_Established ||
        event->InternetConnectivityChange.IPv6 == chip::DeviceLayer::ConnectivityChange::kConnectivity_Established) {
      Serial.println("Device connected to the network.");
    } else {
      Serial.println("Device disconnected from the network.");
    }
  }
}

// Identification callback
static esp_err_t on_identification(identification::callback_type_t type, uint16_t endpoint_id,
                                   uint8_t effect_id, uint8_t effect_variant, void *priv_data) {
  return ESP_OK;
}

// Attribute update callback
static esp_err_t on_attribute_update(attribute::callback_type_t type, uint16_t endpoint_id, uint32_t cluster_id,
                                     uint32_t attribute_id, esp_matter_attr_val_t *val, void *priv_data) {
  if (type == attribute::PRE_UPDATE && endpoint_id == light_endpoint_id &&
      cluster_id == CLUSTER_ID && attribute_id == ATTRIBUTE_ID) {
    bool new_state = val->val.b;
    digitalWrite(LED_PIN, new_state ? HIGH : LOW);
    Serial.printf("LED state updated to: %s\n", new_state ? "ON" : "OFF");
  }
  return ESP_OK;
}

// Function to print QR code for commissioning
void printQRCode() {
  Serial.println("Generating QR Code for Matter Onboarding...");
  chip::RendezvousInformationFlags rendezvousFlags(chip::RendezvousInformationFlag::kOnNetwork);
  PrintOnboardingCodes(rendezvousFlags); // Adjusted for void return type
}

// Reset device to factory defaults
void resetDevice() {
  Serial.println("Factory reset initiated...");
  chip::DeviceLayer::ConfigurationMgr().InitiateFactoryReset();
}

void setup() {
  Serial.begin(115200);
  while (!Serial);
  Serial.println("Serial Monitor is ready");

  // Configure pins
  pinMode(LED_PIN, OUTPUT);
  pinMode(TOGGLE_BUTTON_PIN, INPUT_PULLUP);

  // Debug logging
  esp_log_level_set("*", ESP_LOG_DEBUG);

  // Set up Matter node
  node::config_t node_config;
  node_t *node = node::create(&node_config, on_attribute_update, on_identification);

  // Set up Light endpoint
  on_off_light::config_t light_config;
  light_config.on_off.on_off = true; // Light ON by default
  light_config.on_off.lighting.start_up_on_off = true;
  endpoint_t *endpoint = on_off_light::create(node, &light_config, ENDPOINT_FLAG_NONE, NULL);

  // Save references
  attribute_ref = attribute::get(cluster::get(endpoint, CLUSTER_ID), ATTRIBUTE_ID);
  light_endpoint_id = endpoint::get_id(endpoint);

  // Configure device attestation
  chip::Credentials::SetDeviceAttestationCredentialsProvider(
      chip::Credentials::Examples::GetExampleDACProvider());

  esp_matter::start(on_device_event);

  printQRCode();

  // Set LED ON by default
  Serial.println("Setting LED ON by default...");
  digitalWrite(LED_PIN, HIGH);
  esp_matter_attr_val_t onoff_value = esp_matter_bool(true);
  attribute::update(light_endpoint_id, CLUSTER_ID, ATTRIBUTE_ID, &onoff_value);
}

void loop() {
  if ((millis() - last_toggle) > DEBOUNCE_DELAY) {
    if (!digitalRead(TOGGLE_BUTTON_PIN)) {
      last_toggle = millis();
      esp_matter_attr_val_t onoff_value = esp_matter_invalid(NULL);
      attribute::get_val(attribute_ref, &onoff_value);
      onoff_value.val.b = !onoff_value.val.b;
      attribute::update(light_endpoint_id, CLUSTER_ID, ATTRIBUTE_ID, &onoff_value);
    }
  }
}


