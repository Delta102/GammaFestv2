const d = new Date();
function formatDate(date, format) {
    const map = {
        mm: date.getMonth() + 1,
        dd: date.getDate(),
        yy: date.getFullYear().toString().slice(-2),
        yyyy: date.getFullYear(),
    }
    return format.replace(/mm|dd|yy|yyy/gi, matched => map[matched])
}
formatDate(d, 'mm/dd/yy');
var nombre='testCypress'+ d.toString();
var evento='evento'+ d.toString();
evento=evento.replace(/\s+/g, '').substring(0,10);

/*describe('RegistroEvento', ()=>{
    it('RegistroEvento', ()=>{
        cy.visit(Cypress.env("base_url")+'/Promotor/Login');
        cy.get('#Input_Email').type('admin@admin.pe')
        cy.get('#Input_Password').type('admin')
        cy.get('#login-submit').click()
        cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido')
        cy.get('.navbar-toggler-icon').click()
        cy.get('#create_event').click()
        cy.get('#summernote1').type(evento)
        cy.get('#NombreEvento').type(evento)
        cy.get('#Protocolos').type(evento)
        cy.get('#fechaEvento').type("2025-11-06T13:34")
        cy.get('#ArchivoImagen').click()
        cy.get('input[type=file]').selectFile('1082529.png')
        cy.get('#aforoMaximo').type('900')
        cy.get('#precioEntrada').type('30')
        cy.get('#create').click()
    })
})*/

describe('VerEventos', ()=>{
    it('SelecciónEvento', ()=>{
        cy.visit(Cypress.env("base_url")+'/Promotor/Login');
        cy.get('#Input_Email').type('admin@admin.pe')
        cy.get('#Input_Password').type('admin')
        cy.get('#login-submit').click()
        cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido')
        cy.get('#6').click();
        //cy.visit(Cypress.env("base_url")+'/Evento/IndexEvento/2?id2=15');
        cy.get('#headingOne').click()
        cy.get('#headingTwo').click()
        cy.get('#headingThree').click()
        cy.get('#headingTree').click()
    })
});