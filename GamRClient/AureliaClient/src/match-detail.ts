import {inject} from 'aurelia-framework';
import {EventAggregator} from 'aurelia-event-aggregator';
import {Api} from './api';
import {areEqual} from './utility';
// import {MatchUpdated,MatchViewed} from './messages';

interface IDictionary {
    add(key: string, value: any): void;
    remove(key: string): void;
    containsKey(key: string): boolean;
    keys(): string[];
    values(): any[];
}

interface Match {
  date: Date;
  location: string;
  games: Array<Game>;
  players: Array<string>;
}

interface Game {
  meldingPlayer: string;
  melding: string;
  numberOfVips: number;
  meldedTricks: Array<number>;
  result : {[player: string] : number};
}


@inject(Api,EventAggregator)
export class MatchDetail {
  routeConfig;
  match: Match;
  originalMatch: Match;

  constructor(private api: Api, private ea: EventAggregator) { }

  activate(params, routeConfig) {
    this.routeConfig = routeConfig;

    return this.api.getMatchDetails(params.id).then(match => {
      this.match = <Match> match;
      this.routeConfig.navModel.setTitle(this.match.date);
      this.originalMatch = JSON.parse(JSON.stringify(this.match));
      // this.ea.publish(new MatchViewed(this.match));
    });
  }

  get canSave() {
    return this.match.date && this.match.location && !this.api.isRequesting;
  }

  save() {
    this.api.saveMatch(this.match).then(match => {
      this.match = <Match> match;
      this.routeConfig.navModel.setTitle(this.match.date);
      this.originalMatch = JSON.parse(JSON.stringify(this.match));
      // this.ea.publish(new MatchUpdated(this.match));
    });
  }

  canDeactivate() {
    if(!areEqual(this.originalMatch, this.match)){
      let result = confirm('You have unsaved changes. Are you sure you wish to leave?');

      if(!result) {
        // this.ea.publish(new MatchViewed(this.match));
      }

      return result;
    }

    return true;
  }
}