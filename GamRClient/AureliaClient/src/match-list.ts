import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {Api} from './api';
import {Router, RouterConfiguration} from 'aurelia-router';
// import {MatchUpdated,MatchViewed} from './messages';

@inject(Api,EventAggregator)
export class MatchList {
  matches;
  selectedId = 0;
  constructor(private api: Api, ea: EventAggregator) {
    //     ea.subscribe(PlayerViewed, msg => this.select(msg.player));
    //     ea.subscribe(PlayerUpdated, msg => {
    //       let id = msg.player.id;
    //       let found = this.players.find(x => x.id == id);
    //       Object.assign(found, msg.player);
    // });
   }

  // configureRouter(config: RouterConfiguration, router: Router){
  //   config.title = 'Matches';
  //   config.map([
  //     { route: 'matches/:id',  moduleId: 'match-detail', name:'matches'}
  //   ]);
  //   // config.map([
  //   //   { route: '',              moduleId: 'no-selection',   title: 'Select'},
  //   //   { route: 'players/:id',  moduleId: 'player-detail', name:'players'},
  //   //   { route: 'matches/:id',  moduleId: 'match-detail', name:'matches'}
  //   // ]);
  // }

 activate(params, routeConfig) {
    this.api.getMatchList().then(matches => {
         this.matches = matches
     });
 }
  created() {
     this.api.getMatchList().then(matches => {
         this.matches = matches
     });
  }

  select(match) {
    this.selectedId = match.id;
    return true;
  }
}