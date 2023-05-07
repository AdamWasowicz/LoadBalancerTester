import { sleep, check } from 'k6';
import { Options } from 'k6/options';
import http from 'k6/http';
import * as aE from '../../assets/apiEndpoints';

// Params
const amountOfSeededItems: number = 100;
const VUS: number = 10;
const duration: string = '30s';

export function setup() { 
    const res = http.post(aE.SeedEndpointRoute(aE.ControllersName.COMPANY, amountOfSeededItems))
    return;
}

export function teardown() {
    const res = http.del(aE.CleanUpEndpointRoute());
    return;
}

export let options:Options = {
  vus: VUS,
  duration: duration,
};

export default () => {
    const res = http.get(aE.GetEndpointRoute(aE.ControllersName.COMPANY));
    check(res, {
        'status is 200': () => res.status === 200,
        'status is not 200': () => res.status !== 200
    })
};