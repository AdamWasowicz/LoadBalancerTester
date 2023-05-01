const apiRoutePrefix: string = "http://localhost:30010/api/v1"

export enum ControllersName {
    ADDRESS = "address",
    COMPANY = "company",
    CONTACT_INFO = "contactInfo",
    PRODUCT = "product",
    PRODUCT_SOLD = "productSold",
    SALE = "sale",
    SUPPLIER = "supplier",
    TESTING = "testing",
    WORKER = "worker"
};

export function SeedEndpointRoute(controllerName: ControllersName, amount: number): string {
    return `${apiRoutePrefix}/${controllerName}/seed/${amount}`
}

export function GetEndpointRoute(controllerName: ControllersName): string {
    return `${apiRoutePrefix}/${controllerName}`;
}

export function CleanUpEndpointRoute(): string {
    return `${apiRoutePrefix}/testing`
}