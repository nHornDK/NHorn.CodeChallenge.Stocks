import React from 'react';
import './index.scss';
import { RouteComponentProps, withRouter } from 'react-router';
import TopNavigationBar, {
  TopNavigationBarItem,
} from '../../../../components/navigation/TopNavigationBar';
import { ApplicationRoute, applicationRoutes } from '../../applicationRoutes';

interface HeaderProps extends RouteComponentProps {
  applicationRoutes?: ApplicationRoute[];
}
class Header extends React.PureComponent<HeaderProps> {
  public render(): JSX.Element {
    const { location } = this.props;
    return (
      <header>
        <TopNavigationBar
          currentPath={location.pathname}
          items={applicationRoutes.map(
            (ar: ApplicationRoute): TopNavigationBarItem => ({
              label: ar.label,
              icon: ar.icon,
              path: ar.path,
            })
          )}
        />
      </header>
    );
  }
}

export default withRouter(Header);
