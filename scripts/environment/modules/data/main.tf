#################################################################
# Setup Search on Azure
#################################################################
variable "env_suffix"          {}
variable "location"            {}
variable "sql_server_password" {}

#################################################
# Resource Group
#################################################
resource "azurerm_resource_group" "rg" {
    name                      = "rg-rosterd-data-${var.env_suffix}"
    location                  = var.location
}

#################################################
# Azure Search
#################################################
resource "azurerm_search_service" "search" {
  name                = "rosterd-search-${var.env_suffix}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "basic"
}
#################################################

#################################################
# Application Insights
#################################################
resource "azurerm_application_insights" "insights" {
  name                = "appi-rosterd-logs-${var.env_suffix}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  application_type    = "web"
}
#################################################

#################################################
# MSSQL Resources
#################################################
resource "azurerm_storage_account" "storageacct" {
  name                     = "sqldatabaserosterd${var.env_suffix}"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_mssql_server" "sqlserver" {
  name                         = "sqls-rosterd-${var.env_suffix}"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "rosterd"
  administrator_login_password = var.sql_server_password
}

resource "azurerm_mssql_database" "sqldatabase" {
  name           = "rosterd"
  server_id      = azurerm_mssql_server.sqlserver.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  license_type   = "LicenseIncluded"
  max_size_gb    = 4
  read_scale     = true
  sku_name       = "BC_Gen5_2"
  zone_redundant = true

  extended_auditing_policy {
    storage_endpoint                        = azurerm_storage_account.storageacct.primary_blob_endpoint
    storage_account_access_key              = azurerm_storage_account.storageacct.primary_access_key
    storage_account_access_key_is_secondary = true
    retention_in_days                       = 6
  }

  tags = {
    env = var.env_suffix
  }

}
#################################################