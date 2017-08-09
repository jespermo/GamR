import { Router, RouterConfiguration, RouteConfig, NavModel } from 'aurelia-router';
import { relativeToFile } from 'aurelia-path';
import { CompositionEngine, CompositionContext } from 'aurelia-templating';
import { autoinject } from 'aurelia-framework';
import { Origin } from 'aurelia-metadata';

export class App {
  router: Router;

  constructor(private compositionEngine: CompositionEngine) { }

  configureRouter(config: RouterConfiguration, router: Router){
    config.title = 'Whist';
    config.map([
      { route: '',              moduleId: 'no-selection',  title: 'Select'},
      { route: 'players',  moduleId: 'player-list', name:'players', nav: true, title: "players"},
      { route: 'matches',  moduleId: 'match-list', name:'matches', nav: true, title: "matches"}
    ]);
    // config.map([
    //   { route: '',              moduleId: 'no-selection',   title: 'Select'},
    //   { route: 'players/:id',  moduleId: 'player-detail', name:'players'},
    //   { route: 'matches/:id',  moduleId: 'match-detail', name:'matches'}
    // ]);

    this.router = router;
  }

  // public mapNavigation(router: Router, config?: RouteConfig) {
  //   let promises = [];
  //   let c = config ? config : {route: null};
  //   router.navigation.forEach( nav => {
  //     if (c.route !== nav.config.route) {
  //       promises.push(this.mapNavigationItem(nav, router));
  //     } else {
  //       promises.push(Promise.resolve(nav));
  //     }

  //   })
  //   return Promise.all(promises)
  // }

  // public mapNavigationItem(nav: NavModel, router: Router) {
  //  const config = <any>nav.config
  //   const navModel = nav

  //   if (config.moduleId) {
  //     const childContainer = router.container.createChild();
  //     const instruction = {
  //       viewModel: relativeToFile(config.moduleId, Origin.get(router.container.viewModel.constructor).moduleId),
  //       childContainer: childContainer,
  //       view: config.view || config.viewStrategy,
  //     };
  //     return this.compositionEngine.ensureViewModel(<any>instruction)
  //     .then((context: CompositionContext) => {
  //       if ('configureRouter' in context.viewModel) {
  //         const childRouter = new Router(childContainer, router.history)
  //         const childConfig = new RouterConfiguration()

  //         context.viewModel.configureRouter(childConfig, childRouter)
  //         childConfig.exportToRouter(childRouter)

  //         childRouter.navigation.forEach( nav => {
  //           nav.href = `${navModel.href}/${nav.config.href ? nav.config.href : nav.config.name}`
  //         })
  //         return this.mapNavigation(childRouter, config)
  //           .then(r => navModel.navigation = r)
  //           .then( () => navModel);
  //       }
  //       return navModel
  //     })
  //   }
  //   return Promise.resolve(navModel);
  // }
}