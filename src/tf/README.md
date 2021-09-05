# Deployments on Azure
Azure Rosterd ...

## Components


## Prerequisitess
To run the infrastcurture code, you will need [Teraform](https://www.terraform.io/intro/index.html) installed.


### Usage

In the terraform directory, run:

1. *terraform init* to initialize your terraform directory
2. *terraform plan -var-file="dev.tfvars"* pass in your vars file
3. *terraform apply -var-file="dev.tfvars" -auto-approve* apply will create your resources
4. *terraform destroy -var-file="dev.tfvars"* use the destroy command to delete all your resources

| -----------------------|------------------------------------------------------|
| Variables              | Description                                          |
| -----------------------|------------------------------------------------------|
| `env_suffix`           | *xyz*                                                |
| -----------------------|------------------------------------------------------|

## Manual steps


### Helpful Commands

- *az login*

#### To Do
- 


### Improvements
- 
