#################################################################
# Setup Search on Azure
#################################################################
variable "env_suffix"          {}
variable "location"            {}


#################################################
# Resource Group
#################################################
resource "azurerm_resource_group" "rg" {
    name                      = "rg-rosterd-events-${var.env_suffix}"
    location                  = var.location
}

#################################################
# Azure Event Grid topic
#################################################
resource "azurerm_eventgrid_topic" "events" {
  name                = "evgt-rosterd-${var.env_suffix}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  tags = {
    env = var.env_suffix
  }
}
#################################################


#################################################
# Azure Function App
#################################################
resource "azurerm_storage_account" "storageacct" {
  name                     = "functionsapprosterd${var.env_suffix}"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "appplan" {
  name                = "azure-functions-${var.env_suffix}-service-plan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "funcapp" {
  name                       = "func-event-processor-rush-${var.env_suffix}"
  location                   = azurerm_resource_group.rg.location
  resource_group_name        = azurerm_resource_group.rg.name
  app_service_plan_id        = azurerm_app_service_plan.appplan.id
  storage_account_name       = azurerm_storage_account.storageacct.name
  storage_account_access_key = azurerm_storage_account.storageacct.primary_access_key
}
#################################################
