describe('ComprarEntrada', ()=>{
    it ('AsistirEvento', ()=>{
        cy.visit(Cypress.env("base_url")+'/Promotor/Login');
        cy.get('#Input_Email').type('admin@admin.pe')
        cy.get('#Input_Password').type('admin')
        cy.get('#login-submit').click()
        cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido Admin!')
        cy.get('#2').click();
        cy.get('#venta').click();
        cy.get('#TargetCred').type('2345423243')
        cy.get('#cvv').type('421')
        cy.get('#fechaEvento')
        cy.get('#dia').select('01')
        cy.get('#año').select('2025')
        cy.get('#cantidad')
        cy.get('#select').select('1')
        cy.get('#generar').click()
        cy.get('#descarga').click()
        cy.get('#regresar').click()
    })
})

describe('HistorialEntradas', ()=>{
    it ('EntradaPrincipal', ()=>{
        cy.visit(Cypress.env("base_url")+'/Promotor/Login');
        cy.get('#Input_Email').type('user@user.pe')
        cy.get('#Input_Password').type('user')
        cy.get('#login-submit').click()
        cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido User!')
        cy.get('.navbar-toggler-icon').click()
        cy.get('#create_event').click()
        cy.get('.table')
    })
    it ('EntradaSecundaria', ()=>{
        cy.visit(Cypress.env("base_url")+'/Promotor/Login');
        cy.get('#Input_Email').type('user@user.pe')
        cy.get('#Input_Password').type('user')
        cy.get('#login-submit').click()
        cy.get('.d-flex > ol > .dropdown-item').should('contain.text', 'Bienvenido User!')
        cy.get('.navbar-toggler-icon').click()
        cy.get('#create_event').click()
        cy.get('#entrada_9').click()
        cy.get('#eventot').should('have.text', 'Evento')
        cy.get('#descarga').click()
        cy.get('#regresar').click()
    })
})