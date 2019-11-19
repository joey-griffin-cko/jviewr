import JiraField from "./JiraField";

export default class SprintTask {
    constructor(url: string, key: string, fields: JiraField) {
        this.key = key;
        this.url = url;
        this.fields = fields;
    }
    key: string;
    url: string;
    fields: JiraField
}