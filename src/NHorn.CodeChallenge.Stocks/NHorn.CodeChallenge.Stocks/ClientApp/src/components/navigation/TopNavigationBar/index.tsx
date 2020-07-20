import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import './index.scss';
import { Link } from 'react-router-dom';
import _ from 'lodash';

export interface TopNavigationBarItem {
  icon?: React.ReactElement;
  label: string;
  path: string;
}

export interface TopNavigationBarProps {
  currentPath: string;
  items: TopNavigationBarItem[];
}

class TopNavigationBar extends React.PureComponent<TopNavigationBarProps> {
  public render(): JSX.Element {
    const { items, currentPath } = this.props;
    const selectedValue = _.findIndex(items, (it) => it.path === currentPath);
    return (
      <header>
        <AppBar position="static">
          {selectedValue >= 0 ? (
            <Tabs value={selectedValue}>
              {items.map((item, i) => (
                <TabLink key={item.path} item={item} value={i} />
              ))}
            </Tabs>
          ) : null}
        </AppBar>
      </header>
    );
  }
}
interface TabLinkProps {
  item: TopNavigationBarItem;
  value: number;
}
function TabLink({ item, value }: TabLinkProps) {
  return (
    <Tab
      value={value}
      key={item.path}
      to={item.path}
      component={Link}
      icon={item.icon}
      label={item.label}
    />
  );
}

export default TopNavigationBar;
