#################################################################
# Setup Search on Azure
#################################################################
variable "env_suffix"          {}
variable "location"            {}


#################################################
# Resource Group
#################################################
resource "azurerm_resource_group" "rg" {
    name                      = "rg-rosterd-web-${var.env_suffix}"
    location                  = var.location
}


#################################################
# Azure App Service Plan
#################################################
resource "azurerm_app_service_plan" "plan" {
  name                = "asp-rosterd-plan-common"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  sku {
    tier = "Standard"
    size = "S1"
  }
}

#################################################
# Azure App Service Admin
#################################################
resource "azurerm_app_service" "admin" {
  name                = "app-rosterd-admin-api-${var.env_suffix}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.plan.id

  site_config {
    dotnet_framework_version = "v4.0"
    scm_type                 = "LocalGit"
  }

  app_settings = {
    "SOME_KEY" = "some-value"
  }

  # connection_string {
  #   name  = "Database"
  #   type  = "SQLServer"
  #   value = "xyz"
  # }
}
#################################################

#################################################
# Azure App Service Client
#################################################
resource "azurerm_app_service" "client" {
  name                = "app-rosterd-client-api-${var.env_suffix}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.plan.id

  site_config {
    dotnet_framework_version = "v4.0"
    scm_type                 = "LocalGit"
  }

  app_settings = {
    "SOME_KEY" = "some-value"
  }

  # connection_string {
  #   name  = "Database"
  #   type  = "SQLServer"
  #   value = "xyz"
  # }
}
#################################################
