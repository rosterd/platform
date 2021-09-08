variable "env_suffix"           {}
variable "location"             {}
variable "sql_server_password"  {}
variable "az_subscription_id"   {}
variable "az_client_id"         {}
variable "az_client_secret"     {}
variable "az_tenant_id"         {}
variable "terraform_state"      {}

terraform {
    required_version = "1.0.4"

    required_providers {
      azurerm = {
        source  = "hashicorp/azurerm"
        version = "=2.65.0"
      }
  }
}

provider "azurerm" {
    subscription_id = var.az_subscription_id
    client_id       = var.az_client_id
    client_secret   = var.az_client_secret
    tenant_id       = var.az_tenant_id 

    features {}
}

#############################################################
# Data
#############################################################
module "data" {
source = "./modules/data"

  env_suffix            = var.env_suffix
  location              = var.location
  sql_server_password   = var.sql_server_password
}


#############################################################
# Events
#############################################################
module "events" {
source = "./modules/events"

  env_suffix            = var.env_suffix
  location              = var.location
}

#############################################################
# Web
#############################################################
module "web" {
source = "./modules/web"

  env_suffix            = var.env_suffix
  location              = var.location
}

#############################################################
# Test app service
#############################################################
# data "azurerm_app_service" "example" {
#   name                = "app-rosterd-admin-api-dev"
#   resource_group_name = "rg-rosterd-dev-web"
# }

# output "app_service_id" {
#   value = data.azurerm_app_service.example.id
# }

# output "app_service_app_settings" {
#   value = data.azurerm_app_service.example.app_settings
# }

# output "app_service_app_site_config" {
#   value = data.azurerm_app_service.example.site_config
# }
