import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {Api} from './api';
// import {MatchUpdated,MatchViewed} from './messages';

@inject(Api,EventAggregator)
export class PlayerList {
  players;
  selectedId = 0;
  constructor(private api: Api, ea: EventAggregator) {
    //     ea.subscribe(PlayerViewed, msg => this.select(msg.player));
    //     ea.subscribe(PlayerUpdated, msg => {
    //       let id = msg.player.id;
    //       let found = this.players.find(x => x.id == id);
    //       Object.assign(found, msg.player);
    // });
   }

  created() {
     this.api.getPlayersList().then(matches => {
         this.matches = matches
     });
  }

  select(match) {
    this.selectedId = match.id;
    return true;
  }
}