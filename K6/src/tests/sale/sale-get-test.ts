import { sleep, check } from 'k6';
import { Options } from 'k6/options';
import http from 'k6/http';
import * as aE from '../../assets/apiEndpoints';

// Params
const amountOfSeededItems: number = 50;
const VUS: number = 5;
const duration: string = '60s';

export function setup() {
    const res = http.post(aE.SeedEndpointRoute(aE.ControllersName.SALE, amountOfSeededItems))
    return;
}

export function teardown() {
    const res = http.del(aE.CleanUpEndpointRoute());
    return;
}

export let options:Options = {
  vus: VUS,
  duration: duration
};

export default () => {
    const res = http.get(aE.GetEndpointRoute(aE.ControllersName.SALE));
    check(res, {
        'status is 200': () => res.status === 200,
        'status is not 200': () => res.status !== 200
    })
};