import {Router, RouterConfiguration} from 'aurelia-router';

export class App {
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router){
    config.title = 'Players';
    config.map([
      { route: '',              moduleId: 'no-selection',   title: 'Select'},
      { route: 'players/:id',  moduleId: 'player-detail', name:'players' }
    ]);

    this.router = router;
  }
}