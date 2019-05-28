import * as moment from 'moment';

export class RequestHelper {

    private statuses = [
        { value: 1, text: 'Saved to Azure' },
        { value: 2, text: 'Captured' },
        { value: 3, text: 'Pending' },
        { value: 4, text: 'Approved' }
    ];

    private types = [
        { value: 1, text: 'Peer' },
        { value: 2, text: 'TBD' },
        { value: 3, text: 'IME' },
        { value: 4, text: 'IMEO' },
        { value: 5, text: 'Fee' },
        { value: 6, text: 'Feep' },
        { value: 7, text: 'Court' }
    ];


    getRequestStatus(value: number) {
        const data = this.statuses.find(d => d.value === value);
        return data ? data.text : 'Unknown';
    }

    getRequestType(value: number) {
        const data = this.types.find(d => d.value === value);
        return data ? data.text : 'Unknown';
    }

    formatDate(isoString: string) {
        return isoString ? moment(isoString).format('MM/DD/YYYY') : null;
    }

    get requestStatuses() { return this.statuses; }

    getIsoDateString(dateMoment: moment.Moment|string): string {
        if (typeof dateMoment === 'string') {
            return dateMoment;
        }
        if (dateMoment) {
            return dateMoment.toISOString();
        }
        return null;
    }
}
