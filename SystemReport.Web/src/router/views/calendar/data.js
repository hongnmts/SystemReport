const categories = [
    {
        name: 'Danger',
        value: 'bg-danger'
    },
    {
        name: 'Success',
        value: 'bg-success'
    },
    {
        name: 'Primary',
        value: 'bg-primary'
    },
    {
        name: 'Info',
        value: 'bg-info'
    },
    {
        name: 'Dark',
        value: 'bg-dark'
    },
    {
        name: 'Warning',
        value: 'bg-warning'
    },
];

var date = new Date();
var d = date.getDate();
var m = date.getMonth();
var y = date.getFullYear();

const calendarEvents = [
    {
        title: 'All Day Event',
        start: new Date(y, m, 1),
        className: 'bg-primary text-white',
    },
    {
        title: 'Long Event',
        start: new Date(y, m, d - 5),
        end: new Date(y, m, d - 2),
        className: 'bg-warning text-white',
    },
    {
        id: 999,
        title: 'Repeating Event',
        start: new Date(y, m, d - 3, 16, 0),
        allDay: false,
        className: 'bg-info text-white',
    },
    {
        id: 999,
        title: 'Repeating Event',
        start: new Date(y, m, d + 4, 16, 0),
        allDay: false,
        className: 'bg-primary text-white',
    },
    {
        title: 'Meeting',
        start: new Date(y, m, d, 10, 30),
        allDay: false,
        className: 'bg-success text-white',
    },
    {
        title: 'Lunch',
        start: new Date(y, m, d, 12, 0),
        end: new Date(y, m, d, 14, 0),
        allDay: false,
        className: 'bg-danger text-white',
    },
    {
        title: 'Birthday Party',
        start: new Date(y, m, d + 1, 19, 0),
        end: new Date(y, m, d + 1, 22, 30),
        allDay: false,
        className: 'bg-success text-white',
    }
];

export { categories, calendarEvents };